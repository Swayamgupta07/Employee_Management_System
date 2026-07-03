import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportService } from '../../services/report.service';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="page-header stagger">
      <div>
        <h2 class="page-title fade-in-up">Export Reports</h2>
        <p class="page-subtitle fade-in-up" style="animation-delay: 0.1s">Download employee data in various formats</p>
      </div>
    </div>

    <div class="row g-4 stagger">
      <div class="col-md-6 fade-in-up" style="animation-delay: 0.2s">
        <div class="card h-100 border-0 shadow-sm p-4 text-center">
          <div class="display-3 mb-3">📊</div>
          <h4 class="fw-bold">Excel Roster</h4>
          <p class="text-muted mb-4">Download a full list of all active and inactive employees, including their contact details, department, and salary information formatted as a spreadsheet.</p>
          <button class="btn btn-primary btn-lg w-100" (click)="downloadExcel()" [disabled]="excelLoading()">
            <span *ngIf="excelLoading()" class="spinner-border spinner-border-sm me-2"></span>
            Download Excel
          </button>
        </div>
      </div>

      <div class="col-md-6 fade-in-up" style="animation-delay: 0.3s">
        <div class="card h-100 border-0 shadow-sm p-4 text-center">
          <div class="display-3 mb-3">📄</div>
          <h4 class="fw-bold">PDF Report</h4>
          <p class="text-muted mb-4">Generate a formatted PDF document containing the employee directory, suitable for printing or sharing as an official document.</p>
          <button class="btn btn-outline-primary btn-lg w-100" (click)="downloadPdf()" [disabled]="pdfLoading()">
            <span *ngIf="pdfLoading()" class="spinner-border spinner-border-sm me-2"></span>
            Download PDF
          </button>
        </div>
      </div>
    </div>
  `
})
export class ReportsComponent {
  private reportSvc = inject(ReportService);

  excelLoading = signal(false);
  pdfLoading = signal(false);

  downloadExcel(): void {
    this.excelLoading.set(true);
    this.reportSvc.downloadEmployeeExcel().subscribe({
      next: (blob) => {
        this.reportSvc.triggerDownload(blob, `Employees_Export_${new Date().getTime()}.xlsx`);
        this.excelLoading.set(false);
      },
      error: () => {
        alert('Failed to download Excel report.');
        this.excelLoading.set(false);
      }
    });
  }

  downloadPdf(): void {
    this.pdfLoading.set(true);
    this.reportSvc.downloadEmployeePdf().subscribe({
      next: (blob) => {
        this.reportSvc.triggerDownload(blob, `Employees_Report_${new Date().getTime()}.pdf`);
        this.pdfLoading.set(false);
      },
      error: () => {
        alert('Failed to download PDF report.');
        this.pdfLoading.set(false);
      }
    });
  }
}
