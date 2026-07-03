import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AttendanceService } from '../../services/attendance.service';
import { AuthService } from '../../services/auth.service';
import { Attendance } from '../../models/attendance.model';
import { AttendanceFormComponent } from './attendance-form.component';

@Component({
  selector: 'app-attendance-list',
  standalone: true,
  imports: [CommonModule, FormsModule, AttendanceFormComponent],
  templateUrl: './attendance-list.component.html'
})
export class AttendanceListComponent implements OnInit {
  private attSvc = inject(AttendanceService);
  authSvc = inject(AuthService);

  records = signal<Attendance[]>([]);
  loading = signal(true);
  
  // Filtering
  filterType = signal<'all'|'employee'|'date'|'month'>('month');
  empIdInput = signal('');
  startDateInput = signal('');
  endDateInput = signal('');
  monthInput = signal(new Date().getMonth() + 1);
  yearInput = signal(new Date().getFullYear());

  showModal = signal(false);
  selectedRecord = signal<Attendance | null>(null);

  canEdit = this.authSvc.hasRole('Admin', 'HR');

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading.set(true);
    const type = this.filterType();

    let req$;
    if (type === 'all') req$ = this.attSvc.getAll();
    else if (type === 'employee' && this.empIdInput()) req$ = this.attSvc.getByEmployee(+this.empIdInput());
    else if (type === 'date' && this.startDateInput() && this.endDateInput()) req$ = this.attSvc.getByDateRange(this.startDateInput(), this.endDateInput());
    else if (type === 'month') req$ = this.attSvc.getMonthlyReport(this.monthInput(), this.yearInput());
    else { this.loading.set(false); return; }

    req$.subscribe({
      next: (data) => {
        this.records.set(Array.isArray(data) ? data : []);
        this.loading.set(false);
      },
      error: () => {
        this.records.set([]);
        this.loading.set(false);
      }
    });
  }

  openCreateModal(): void {
    this.selectedRecord.set(null);
    this.showModal.set(true);
  }

  openEditModal(rec: Attendance): void {
    this.selectedRecord.set(rec);
    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
    this.selectedRecord.set(null);
  }

  onSaved(): void {
    this.closeModal();
    this.loadData();
  }

  delete(id: number): void {
    if(confirm('Delete this attendance record?')) {
      this.attSvc.delete(id).subscribe({
        next: () => this.loadData(),
        error: (err) => alert('Error deleting record')
      });
    }
  }

  getStatusClass(status: string): string {
    const s = status.toLowerCase();
    if (s.includes('present')) return 'badge-active';
    if (s.includes('absent')) return 'badge-inactive';
    if (s.includes('late')) return 'badge-warning';
    return 'bg-light text-dark';
  }
}
