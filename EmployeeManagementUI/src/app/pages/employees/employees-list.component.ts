import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EmployeeService } from '../../services/employee.service';
import { DepartmentService } from '../../services/department.service';
import { AuthService } from '../../services/auth.service';
import { Employee } from '../../models/employee.model';
import { Department } from '../../models/department.model';
import { EmployeeFormComponent } from './employee-form.component';

@Component({
  selector: 'app-employees-list',
  standalone: true,
  imports: [CommonModule, FormsModule, EmployeeFormComponent],
  templateUrl: './employees-list.component.html',
  styleUrl: './employees-list.component.css'
})
export class EmployeesListComponent implements OnInit {
  employeeSvc = inject(EmployeeService);
  deptSvc = inject(DepartmentService);
  authSvc = inject(AuthService);

  Math = Math; // Expose to template

  employees = signal<Employee[]>([]);
  departments = signal<Department[]>([]);
  
  totalRecords = signal(0);
  pageNumber = signal(1);
  pageSize = signal(10);
  
  loading = signal(true);
  
  // Search
  searchName = signal('');
  searchEmail = signal('');
  searchDept = signal('');

  // Modal State
  showModal = signal(false);
  selectedEmployee = signal<Employee | null>(null);
  
  canEdit = this.authSvc.hasRole('Admin', 'HR');

  ngOnInit(): void {
    this.loadDepartments();
    this.loadEmployees();
  }

  loadDepartments(): void {
    this.deptSvc.getAll().subscribe(res => {
      if (res.success) this.departments.set(res.data);
    });
  }

  loadEmployees(): void {
    this.loading.set(true);
    this.employeeSvc.getAll(this.pageNumber(), this.pageSize()).subscribe({
      next: (res) => {
        if (res.success) {
          this.employees.set(res.data);
          this.totalRecords.set(res.totalRecords);
        }
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  onSearch(): void {
    if (!this.searchName() && !this.searchEmail() && !this.searchDept()) {
      this.pageNumber.set(1);
      this.loadEmployees();
      return;
    }

    this.loading.set(true);
    this.employeeSvc.search({
      name: this.searchName() || undefined,
      email: this.searchEmail() || undefined,
      departmentId: this.searchDept() ? +this.searchDept() : undefined
    }).subscribe({
      next: (res) => {
        if (res.success) {
          this.employees.set(res.data);
          this.totalRecords.set(res.data.length);
        }
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  resetSearch(): void {
    this.searchName.set('');
    this.searchEmail.set('');
    this.searchDept.set('');
    this.pageNumber.set(1);
    this.loadEmployees();
  }

  changePage(delta: number): void {
    const newVal = this.pageNumber() + delta;
    if (newVal < 1 || newVal > Math.ceil(this.totalRecords() / this.pageSize())) return;
    this.pageNumber.set(newVal);
    this.loadEmployees();
  }

  openCreateModal(): void {
    this.selectedEmployee.set(null);
    this.showModal.set(true);
  }

  openEditModal(emp: Employee): void {
    this.selectedEmployee.set(emp);
    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
    this.selectedEmployee.set(null);
  }

  onSaved(): void {
    this.closeModal();
    this.loadEmployees();
  }

  deactivate(id: number): void {
    if(confirm('Are you sure you want to deactivate this employee?')) {
      this.employeeSvc.deactivate(id).subscribe(() => this.loadEmployees());
    }
  }

  delete(id: number): void {
    if(confirm('Are you sure you want to completely delete this employee? This action cannot be undone.')) {
      this.employeeSvc.delete(id).subscribe(() => this.loadEmployees());
    }
  }
}
