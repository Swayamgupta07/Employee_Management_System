import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-otp-verify',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="auth-page">
      <div class="auth-card">
        <div class="auth-logo">
          <div class="logo-icon">🔑</div>
          <h2>Verify Identity</h2>
          <p>Enter the 6-digit code sent to your email</p>
          <p class="small text-primary">{{ email() }}</p>
        </div>

        <div *ngIf="error()" class="alert alert-danger px-3 py-2 text-sm rounded bg-danger bg-opacity-10 border border-danger text-light mb-4">
          {{ error() }}
        </div>

        <div class="otp-input-container mb-4">
           <input type="text" [(ngModel)]="otp" maxlength="6" class="form-control otp-field" placeholder="000000" (keyup.enter)="verify()">
        </div>

        <button (click)="verify()" class="auth-btn" [disabled]="loading() || otp.length !== 6">
          <span *ngIf="loading()" class="spinner-border spinner-border-sm me-2"></span>
          Verify Node Access
        </button>

        <div class="auth-link mt-4 text-center">
          <a href="javascript:void(0)" (click)="goBack()" class="small">Back to Registration</a>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .otp-input-container {
      display: flex;
      justify-content: center;
    }
    .otp-field {
      letter-spacing: 0.5rem;
      text-align: center;
      font-size: 1.5rem;
      font-weight: 700;
      background: rgba(15, 23, 42, 0.6);
      border: 1px solid rgba(255, 255, 255, 0.08);
      color: #f8fafc;
      border-radius: 12px;
    }
    .otp-field:focus {
      border-color: #8b5cf6;
      box-shadow: 0 0 0 3px rgba(139, 92, 246, 0.1);
    }
  `]
})
export class OtpVerifyComponent {
  private auth = inject(AuthService);
  private router = inject(Router);

  email = signal(localStorage.getItem('pending_verify_email') || '');
  otp = '';
  loading = signal(false);
  error = signal('');

  verify(): void {
    if (this.otp.length !== 6) return;
    this.loading.set(true);
    this.error.set('');

    this.auth.verifyOtp(this.email(), this.otp).subscribe({
      next: (res: any) => {
        localStorage.removeItem('pending_verify_email');
        if (res.message.includes('overseer')) {
           this.router.navigate(['/login'], { queryParams: { pendingAdmin: 'true' } });
        } else {
           this.router.navigate(['/dashboard']);
        }
      },
      error: (err) => {
        this.error.set(err?.error?.message || 'Invalid or expired OTP.');
        this.loading.set(false);
      },
      complete: () => this.loading.set(false)
    });
  }

  goBack(): void {
    this.router.navigate(['/register']);
  }
}
