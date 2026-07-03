import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DepartmentService } from '../../services/department.service';
import { EmployeeService } from '../../services/employee.service';
import { AuthService } from '../../services/auth.service';
import { Department } from '../../models/department.model';
import { Employee } from '../../models/employee.model';
import { DepartmentFormComponent } from './department-form.component';

@Component({
  selector: 'app-departments-list',
  standalone: true,
  imports: [CommonModule, DepartmentFormComponent],
  templateUrl: './departments-list.component.html',
  styleUrl: './departments-list.component.css'
})
export class DepartmentsListComponent implements OnInit {
  private deptSvc = inject(DepartmentService);
  private empSvc = inject(EmployeeService);
  authSvc = inject(AuthService);

  departments = signal<Department[]>([]);
  loading = signal(true);
  
  showModal = signal(false);
  selectedDepartment = signal<Department | null>(null);

  // Members panel
  showMembersPanel = signal(false);
  membersPanelDept = signal<Department | null>(null);
  membersList = signal<Employee[]>([]);
  membersLoading = signal(false);

  canEdit = this.authSvc.hasRole('Admin', 'HR');

  getDepartmentIcon(name: string): string {
    if (!name) return 'corporate_fare';
    const lower = name.toLowerCase();
    if (lower.includes('analyst') || lower.includes('analytic')) return 'query_stats';
    if (lower.includes('hr') || lower.includes('human resource')) return 'diversity_3';
    if (lower.includes('finance') || lower.includes('account')) return 'account_balance';
    if (lower.includes('sales') || lower.includes('marketing')) return 'campaign';
    if (lower.includes('it') || lower.includes('information tech') || lower.includes('engineer') || lower.includes('dev')) return 'terminal';
    if (lower.includes('op') || lower.includes('operation')) return 'precision_manufacturing';
    if (lower.includes('support') || lower.includes('service')) return 'support_agent';
    if (lower.includes('admin') || lower.includes('general')) return 'admin_panel_settings';
    if (lower.includes('legal')) return 'gavel';
    return 'corporate_fare';
  }

  getDeptGradient(index: number): string {
    const gradients = [
      'linear-gradient(135deg, #00C9A7, #00A080)',
      'linear-gradient(135deg, #7B61FF, #5B42D0)',
      'linear-gradient(135deg, #3B82F6, #1D5CC7)',
      'linear-gradient(135deg, #F59E0B, #D97706)',
      'linear-gradient(135deg, #EC4899, #BE185D)',
      'linear-gradient(135deg, #EF4444, #B91C1C)',
    ];
    return gradients[index % gradients.length];
  }

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.loading.set(true);
    this.deptSvc.getAll().subscribe({
      next: (res) => {
        if (res.success) this.departments.set(res.data);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  openCreateModal(): void {
    this.selectedDepartment.set(null);
    this.showModal.set(true);
  }

  openEditModal(dept: Department): void {
    this.selectedDepartment.set(dept);
    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
    this.selectedDepartment.set(null);
  }

  onSaved(): void {
    this.closeModal();
    this.loadDepartments();
  }

  delete(id: number): void {
    if (confirm('Are you sure you want to delete this department?')) {
      this.deptSvc.delete(id).subscribe({
        next: (res) => {
          if (res.success) this.loadDepartments();
          else alert(res.message);
        },
        error: (err) => alert(err?.error?.message || 'Error deleting department')
      });
    }
  }

  // View All Members panel
  openMembersPanel(dept: Department): void {
    this.membersPanelDept.set(dept);
    this.showMembersPanel.set(true);
    this.membersList.set([]);
    this.membersLoading.set(true);
    this.empSvc.search({ departmentId: dept.id }).subscribe({
      next: (res) => {
        if (res.success) this.membersList.set(res.data);
        this.membersLoading.set(false);
      },
      error: () => this.membersLoading.set(false)
    });
  }

  closeMembersPanel(): void {
    this.showMembersPanel.set(false);
    this.membersPanelDept.set(null);
    this.membersList.set([]);
  }
}
