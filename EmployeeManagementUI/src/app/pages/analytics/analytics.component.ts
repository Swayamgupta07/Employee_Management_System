import { Component, OnInit, inject, signal, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BonusFeatureService } from '../../services/bonus-feature.service';
import { forkJoin } from 'rxjs';

declare var Chart: any;

@Component({
  selector: 'app-analytics',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './analytics.component.html'
})
export class AnalyticsComponent implements OnInit, AfterViewInit {
  private bonusSvc = inject(BonusFeatureService);

  // Plain strings (not signals) so [(ngModel)] works directly
  startDate = '';
  endDate   = '';
  loading   = signal(false);

  // Empty-state flags per chart
  noHiring     = signal(false);
  noGrowth     = signal(false);
  noAttendance = signal(false);
  noPerf       = signal(false);

  @ViewChild('hiringChart')      hiringChartRef!:     ElementRef<HTMLCanvasElement>;
  @ViewChild('deptGrowthChart')  deptGrowthChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('attendanceChart')  attendanceChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('performanceChart') performanceChartRef!: ElementRef<HTMLCanvasElement>;

  private charts: any[] = [];

  ngOnInit(): void {
    const end   = new Date();
    const start = new Date();
    start.setMonth(start.getMonth() - 6);
    this.startDate = start.toISOString().split('T')[0];
    this.endDate   = end.toISOString().split('T')[0];
  }

  ngAfterViewInit(): void {
    this.loadData();
  }

  loadData(): void {
    if (!this.startDate || !this.endDate) return;
    this.loading.set(true);
    this.noHiring.set(false);
    this.noGrowth.set(false);
    this.noAttendance.set(false);
    this.noPerf.set(false);

    forkJoin({
      hiring:     this.bonusSvc.getHiringTrends(this.startDate, this.endDate),
      growth:     this.bonusSvc.getDepartmentGrowth(this.startDate, this.endDate),
      attendance: this.bonusSvc.getAttendancePatterns(this.startDate, this.endDate),
      perf:       this.bonusSvc.getPerformanceMetrics(this.startDate, this.endDate)
    }).subscribe({
      next: (res) => {
        this.destroyCharts();

        // ── Hiring Trends ─────────────────────────────────────────────
        // API: { year, month, totalHires }
        if (res.hiring.success && res.hiring.data?.length) {
          this.renderHiringChart(res.hiring.data);
        } else {
          this.noHiring.set(true);
        }

        // ── Department Growth ─────────────────────────────────────────
        // API: { departmentId, departmentName, employeeCount, date }
        if (res.growth.success && res.growth.data?.length) {
          this.renderDeptGrowthChart(res.growth.data);
        } else {
          this.noGrowth.set(true);
        }

        // ── Attendance Patterns ───────────────────────────────────────
        // API: { employeeId, employeeName, presentDays, absentDays, lateArrivals }
        if (res.attendance.success && res.attendance.data?.length) {
          this.renderAttendanceChart(res.attendance.data);
        } else {
          this.noAttendance.set(true);
        }

        // ── Performance Metrics ───────────────────────────────────────
        // API: { employeeId, employeeName, attendanceScore, projectScore, overallScore }
        if (res.perf.success && res.perf.data?.length) {
          this.renderPerformanceChart(res.perf.data);
        } else {
          this.noPerf.set(true);
        }

        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  private destroyCharts(): void {
    this.charts.forEach(c => { try { c.destroy(); } catch {} });
    this.charts = [];
  }

  // ── Chart renderers ────────────────────────────────────────────────

  /** API shape: { year: number, month: number, totalHires: number } */
  private renderHiringChart(data: any[]): void {
    const ctx = this.hiringChartRef.nativeElement.getContext('2d')!;
    const labels = data.map(d => `${this.monthName(d.month)} ${d.year}`);
    const chart = new Chart(ctx, {
      type: 'line',
      data: {
        labels,
        datasets: [{
          label: 'New Hires',
          data: data.map(d => d.totalHires),
          borderColor: '#00C9A7',
          backgroundColor: 'rgba(0,201,167,0.12)',
          fill: true,
          tension: 0.4,
          pointBackgroundColor: '#00C9A7',
          pointRadius: 5,
          borderWidth: 2
        }]
      },
      options: this.darkChartOptions('New hires per month')
    });
    this.charts.push(chart);
  }

  /** API shape: { departmentName: string, employeeCount: number } */
  private renderDeptGrowthChart(data: any[]): void {
    const ctx = this.deptGrowthChartRef.nativeElement.getContext('2d')!;
    const chart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: data.map(d => d.departmentName),
        datasets: [{
          label: 'Employees in Period',
          data: data.map(d => d.employeeCount),
          backgroundColor: data.map((_, i) =>
            ['#00C9A7','#7B61FF','#3B82F6','#F59E0B','#EF4444','#EC4899'][i % 6]
          ),
          borderRadius: 6
        }]
      },
      options: this.darkChartOptions('Employees hired per department')
    });
    this.charts.push(chart);
  }

  /** API shape: { employeeName, presentDays, absentDays, lateArrivals } */
  private renderAttendanceChart(data: any[]): void {
    const ctx = this.attendanceChartRef.nativeElement.getContext('2d')!;
    const chart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: data.map(d => d.employeeName),
        datasets: [
          {
            label: 'Present',
            data: data.map(d => d.presentDays),
            backgroundColor: 'rgba(0,201,167,0.8)',
            stack: 'stack',
            borderRadius: 4
          },
          {
            label: 'Late',
            data: data.map(d => d.lateArrivals),
            backgroundColor: 'rgba(245,158,11,0.8)',
            stack: 'stack'
          },
          {
            label: 'Absent',
            data: data.map(d => d.absentDays),
            backgroundColor: 'rgba(239,68,68,0.8)',
            stack: 'stack'
          }
        ]
      },
      options: {
        ...this.darkChartOptions('Days per employee'),
        scales: {
          x: { stacked: true, ticks: { color: '#94A3B8' }, grid: { color: 'rgba(255,255,255,0.05)' } },
          y: { stacked: true, ticks: { color: '#94A3B8' }, grid: { color: 'rgba(255,255,255,0.05)' } }
        }
      }
    });
    this.charts.push(chart);
  }

  /** API shape: { employeeName, attendanceScore, projectScore, overallScore } */
  private renderPerformanceChart(data: any[]): void {
    const ctx = this.performanceChartRef.nativeElement.getContext('2d')!;
    const chart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: data.map(d => d.employeeName),
        datasets: [
          {
            label: 'Attendance Score',
            data: data.map(d => d.attendanceScore),
            backgroundColor: 'rgba(0,201,167,0.8)',
            borderRadius: 4
          },
          {
            label: 'Overall Score',
            data: data.map(d => d.overallScore),
            backgroundColor: 'rgba(123,97,255,0.8)',
            borderRadius: 4
          }
        ]
      },
      options: this.darkChartOptions('Score per employee')
    });
    this.charts.push(chart);
  }

  // ── Helpers ────────────────────────────────────────────────────────

  private darkChartOptions(yLabel = ''): any {
    return {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: { labels: { color: '#94A3B8', font: { family: 'Inter' } } }
      },
      scales: {
        x: { ticks: { color: '#94A3B8' }, grid: { color: 'rgba(255,255,255,0.05)' } },
        y: {
          ticks: { color: '#94A3B8' },
          grid: { color: 'rgba(255,255,255,0.05)' },
          title: { display: !!yLabel, text: yLabel, color: '#4B5563' }
        }
      }
    };
  }

  private monthName(m: number): string {
    return ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][m - 1] ?? `M${m}`;
  }
}
