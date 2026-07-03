import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { EmployeeService } from '../../services/employee.service';
import { DepartmentService } from '../../services/department.service';
import { BonusFeatureService } from '../../services/bonus-feature.service';
import { AuthService } from '../../services/auth.service';
import { Employee } from '../../models/employee.model';
import { Department } from '../../models/department.model';
import { forkJoin } from 'rxjs';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import {
  Chart,
  LineController, LineElement, PointElement, LinearScale, CategoryScale,
  Filler, Tooltip, Legend,
  DoughnutController, ArcElement
} from 'chart.js';

Chart.register(
  LineController, LineElement, PointElement, LinearScale, CategoryScale,
  Filler, Tooltip, Legend,
  DoughnutController, ArcElement
);

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, BaseChartDirective, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  private employeeSvc = inject(EmployeeService);
  private deptSvc = inject(DepartmentService);
  private bonusSvc = inject(BonusFeatureService);
  auth = inject(AuthService);

  get isAdmin(): boolean { return this.auth.hasRole('Admin'); }
  pendingCount = signal(0);

  totalEmployees   = signal(0);
  activeEmployees  = signal(0);
  totalDepartments = signal(0);
  pendingApprovals = signal(0);
  recentEmployees  = signal<Employee[]>([]);
  departments      = signal<Department[]>([]);
  loading          = signal(true);

  activeRange = signal<'7D' | '30D' | '90D'>('30D');

  // ---- Attendance Line Chart ----
  attendanceChartData: ChartData<'line'> = {
    labels: this.getLast30DayLabels(),
    datasets: [
      {
        label: 'Present (%)',
        data: this.generateAttendanceData(30),
        borderColor: '#00C9A7',
        backgroundColor: 'rgba(0,201,167,0.08)',
        fill: true,
        tension: 0.4,
        pointRadius: 3,
        pointHoverRadius: 6,
        pointBackgroundColor: '#00C9A7',
        borderWidth: 2
      },
      {
        label: 'Benchmark',
        data: Array(30).fill(85),
        borderColor: 'rgba(123,97,255,0.5)',
        backgroundColor: 'transparent',
        fill: false,
        tension: 0,
        pointRadius: 0,
        borderDash: [5, 5],
        borderWidth: 1.5
      }
    ]
  };

  attendanceChartOptions: ChartConfiguration<'line'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: true,
        labels: { color: '#94A3B8', font: { size: 11 }, boxWidth: 12, padding: 16 }
      },
      tooltip: {
        backgroundColor: '#1A2233',
        titleColor: '#F0F4FF',
        bodyColor: '#94A3B8',
        borderColor: 'rgba(255,255,255,0.07)',
        borderWidth: 1
      }
    },
    scales: {
      x: {
        grid: { color: 'rgba(255,255,255,0.04)' },
        ticks: { color: '#4B5563', font: { size: 10 }, maxTicksLimit: 10 }
      },
      y: {
        min: 60, max: 100,
        grid: { color: 'rgba(255,255,255,0.04)' },
        ticks: { color: '#4B5563', font: { size: 10 }, callback: (v) => v + '%' }
      }
    }
  };

  // ---- Department Donut Chart ----
  deptChartData: ChartData<'doughnut'> = {
    labels: [],
    datasets: [{
      data: [],
      backgroundColor: ['#00C9A7','#7B61FF','#3B82F6','#F59E0B','#EC4899','#EF4444'],
      borderColor: '#111827',
      borderWidth: 3,
      hoverOffset: 6
    }]
  };

  deptChartOptions: ChartConfiguration<'doughnut'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    cutout: '72%',
    plugins: {
      legend: {
        display: false
      },
      tooltip: {
        backgroundColor: '#1A2233',
        titleColor: '#F0F4FF',
        bodyColor: '#94A3B8',
        borderColor: 'rgba(255,255,255,0.07)',
        borderWidth: 1
      }
    }
  };

  deptColors = ['#00C9A7','#7B61FF','#3B82F6','#F59E0B','#EC4899','#EF4444'];

  ngOnInit(): void {
    // Load pending approvals count for admin
    if (this.isAdmin) {
      this.auth.getPendingApprovals().subscribe({
        next: (res) => {
          if (res.success && res.data) this.pendingCount.set(res.data.length);
        },
        error: () => {}
      });
    }

    const end = new Date().toISOString();
    const startObj = new Date();
    startObj.setMonth(startObj.getMonth() - 1);
    const start = startObj.toISOString();

    forkJoin({
      employees: this.employeeSvc.getAll(1, 100), // get enough to count active
      depts: this.deptSvc.getAll(),
      metrics: this.bonusSvc.getPerformanceMetrics(start, end)
    }).subscribe({
      next: (res) => {
        const empList = res.employees.data || [];
        this.recentEmployees.set(empList.slice(0, 10));

        // ── KPI counts come from the employees response ──────────────
        this.totalEmployees.set(res.employees.totalRecords || empList.length);
        this.activeEmployees.set(empList.filter((e: Employee) => e.isActive).length);

        const depts = res.depts.data || [];
        this.departments.set(depts);
        this.totalDepartments.set(depts.length);

        // Build donut chart from real departments
        this.deptChartData = {
          labels: depts.map(d => d.name),
          datasets: [{
            data: depts.map(d => d.employeeCount || 0),
            backgroundColor: this.deptColors.slice(0, depts.length),
            borderColor: '#111827',
            borderWidth: 3,
            hoverOffset: 6
          }]
        };

        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });

  }

  setRange(range: '7D' | '30D' | '90D') {
    this.activeRange.set(range);
    const days = range === '7D' ? 7 : range === '30D' ? 30 : 90;
    this.attendanceChartData = {
      ...this.attendanceChartData,
      labels: this.getLastNDayLabels(days),
      datasets: [
        { ...this.attendanceChartData.datasets[0], data: this.generateAttendanceData(days) },
        { ...this.attendanceChartData.datasets[1], data: Array(days).fill(85) }
      ]
    };
  }

  private getLast30DayLabels(): string[] { return this.getLastNDayLabels(30); }

  private getLastNDayLabels(n: number): string[] {
    const labels: string[] = [];
    for (let i = n - 1; i >= 0; i--) {
      const d = new Date();
      d.setDate(d.getDate() - i);
      labels.push(d.toLocaleDateString('en-US', { month: 'short', day: 'numeric' }));
    }
    return labels;
  }

  private generateAttendanceData(n: number): number[] {
    const data: number[] = [];
    for (let i = 0; i < n; i++) {
      data.push(Math.floor(Math.random() * 18) + 78); // 78–95%
    }
    return data;
  }

  get deptTotal(): number {
    return (this.deptChartData.datasets[0]?.data as number[])?.reduce((a, b) => a + b, 0) || 0;
  }
}
