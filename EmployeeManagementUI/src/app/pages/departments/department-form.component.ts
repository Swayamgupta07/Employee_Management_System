import { Component, EventEmitter, Input, OnInit, Output, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { DepartmentService } from '../../services/department.service';
import { Department } from '../../models/department.model';

@Component({
  selector: 'app-department-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './department-form.component.html'
})
export class DepartmentFormComponent implements OnInit {
  @Input() department: Department | null = null;
  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter<void>();

  private fb = inject(FormBuilder);
  private deptSvc = inject(DepartmentService);

  form = this.fb.group({
    name: ['', Validators.required],
    description: ['']
  });

  loading = signal(false);
  error = signal('');

  ngOnInit(): void {
    if (this.department) {
      this.form.patchValue({
        name: this.department.name,
        description: this.department.description
      });
    }
  }

  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    
    this.loading.set(true);
    this.error.set('');

    const request$ = this.department 
      ? this.deptSvc.update(this.department.id, this.form.value as any)
      : this.deptSvc.create(this.form.value as any);

    request$.subscribe({
      next: (res) => {
        if (res.success) this.saved.emit();
        else { this.error.set(res.message); this.loading.set(false); }
      },
      error: (err) => {
        this.error.set(err?.error?.message || 'Error saving department');
        this.loading.set(false);
      }
    });
  }
}
