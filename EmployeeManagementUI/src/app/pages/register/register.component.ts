import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { DepartmentService } from '../../services/department.service';
import { Department } from '../../models/department.model';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  private deptService = inject(DepartmentService);

  form = this.fb.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    requestedRole: ['Employee', [Validators.required]],
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    phone: [''],
    position: ['', [Validators.required]],
    departmentId: ['', [Validators.required]]
  });

  departments = signal<Department[]>([]);
  loading = signal(false);
  error = signal('');
  success = signal('');
  
  // 0: Empty, 1: Weak, 2: Fair, 3: Good, 4: Strong
  passwordStrength = signal(0);
  strengthLabel = signal('');
  showPass = signal(false);

  ngOnInit(): void {
    this.form.get('password')?.valueChanges.subscribe(val => {
      this.calculatePasswordStrength(val || '');
    });

    this.deptService.getAll().subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.departments.set(res.data);
        }
      },
      error: (err) => console.error('Failed to load departments', err)
    });
  }

  private calculatePasswordStrength(password: string): void {
    if (!password) {
      this.passwordStrength.set(0);
      this.strengthLabel.set('');
      return;
    }

    let strength = 0;
    if (password.length >= 6) strength++;
    if (password.length >= 10) strength++;
    if (/[A-Z]/.test(password)) strength++;
    if (/[0-9]/.test(password)) strength++;
    if (/[^A-Za-z0-9]/.test(password)) strength++;

    // Cap at 4 for UI
    const finalScore = Math.min(strength, 4);
    this.passwordStrength.set(finalScore);

    const labels = ['', 'Weak', 'Fair', 'Good', 'Strong'];
    this.strengthLabel.set(labels[finalScore]);
  }

  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.loading.set(true);
    this.error.set('');

    this.auth.register(this.form.value as any).subscribe({
      next: (res: any) => {
        // Save email for OTP verification
        localStorage.setItem('pending_verify_email', this.form.value.email || '');
        
        // Navigate to OTP verification page
        this.router.navigate(['/verify-otp']);
      },
      error: (err) => {
        console.error('Registration error:', err);
        // Sometimes ASP.NET validation errors are in err.error.errors
        if (err?.error?.errors) {
          const firstError = Object.values(err.error.errors)[0] as string[];
          this.error.set(firstError[0]);
        } else if (err?.error?.message) {
          this.error.set(err.error.message);
        } else if (err?.message) {
          this.error.set(err.message);
        } else {
          this.error.set('Registration failed. Please try again.');
        }
        this.loading.set(false);
      },
      complete: () => this.loading.set(false)
    });
  }
}
