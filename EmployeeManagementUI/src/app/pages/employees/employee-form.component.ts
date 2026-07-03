import { Component, EventEmitter, Input, OnInit, Output, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { EmployeeService } from '../../services/employee.service';
import { Employee } from '../../models/employee.model';
import { Department } from '../../models/department.model';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './employee-form.component.html'
})
export class EmployeeFormComponent implements OnInit {
  @Input() employee: Employee | null = null;
  @Input() departments: Department[] = [];
  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter<void>();

  private fb = inject(FormBuilder);
  private empSvc = inject(EmployeeService);

  form = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    position: ['', Validators.required],
    departmentId: ['', Validators.required],
    salary: [0, [Validators.required, Validators.min(0)]],
    hireDate: ['', Validators.required]
  });

  loading = signal(false);
  error = signal('');

  ngOnInit(): void {
    if (this.employee) {
      this.form.patchValue({
        firstName: this.employee.firstName,
        lastName: this.employee.lastName,
        email: this.employee.email,
        phone: this.employee.phone,
        position: this.employee.position,
        departmentId: this.employee.departmentId.toString(),
        salary: this.employee.salary,
        // Format date to YYYY-MM-DD for input type="date"
        hireDate: new Date(this.employee.hireDate).toISOString().split('T')[0]
      });
    }
  }

  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    
    this.loading.set(true);
    this.error.set('');

    const dto = {
      ...this.form.value,
      departmentId: Number(this.form.value.departmentId)
    } as any;

    const request$ = this.employee 
      ? this.empSvc.update(this.employee.id, dto)
      : this.empSvc.create(dto);

    request$.subscribe({
      next: (res) => {
        if (res.success) this.saved.emit();
        else { this.error.set(res.message); this.loading.set(false); }
      },
      error: (err) => {
        this.error.set(err?.error?.message || 'Error saving employee');
        this.loading.set(false);
      }
    });
  }
}
