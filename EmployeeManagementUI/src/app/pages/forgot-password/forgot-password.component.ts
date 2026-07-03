import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <!-- Toast notification (top-right) -->
    <div *ngIf="showToast()" class="fp-toast">
      <div class="fp-toast-icon">
        <span class="material-symbols-rounded">mark_email_read</span>
      </div>
      <div class="fp-toast-body">
        <div class="fp-toast-title">Token sent to your email!</div>
        <div class="fp-toast-sub">
          Check your inbox, then continue to reset your password.
        </div>
        <button class="fp-toast-cta" (click)="goToReset()">
          Continue to Reset Password →
        </button>
      </div>
      <button class="fp-toast-close" (click)="showToast.set(false)">
        <span class="material-symbols-rounded">close</span>
      </button>
      <!-- Progress bar countdown -->
      <div class="fp-toast-progress" [style.animation-duration]="'4s'"></div>
    </div>

    <div class="auth-page">
      <div class="auth-card">
        <div class="auth-logo">
          <div class="logo-icon">🔐</div>
          <h2>Forgot Password</h2>
          <p>Enter your email and we'll send a reset token</p>
        </div>

        <div *ngIf="error()" class="alert alert-danger mb-4">{{ error() }}</div>

        <form [formGroup]="form" (ngSubmit)="submit()">
          <div class="mb-4">
            <label class="form-label">Email</label>
            <input
              formControlName="email"
              type="email"
              class="form-control"
              placeholder="you@company.com"
              [disabled]="loading()">
          </div>
          <button type="submit" class="auth-btn" [disabled]="loading() || showToast()">
            <span *ngIf="!loading()">Send Reset Link</span>
            <span *ngIf="loading()">Sending…</span>
          </button>
        </form>

        <p class="auth-link">
          Remember your password? <a routerLink="/login">Back to Login</a>
        </p>
      </div>
    </div>
  `,
  styles: [`
    /* ── Toast ───────────────────────────────────────────────── */
    .fp-toast {
      position: fixed;
      top: 24px; right: 24px;
      width: 340px;
      background: #1A2233;
      border: 1px solid rgba(0,201,167,0.35);
      border-radius: 14px;
      padding: 18px 16px 14px;
      box-shadow: 0 12px 48px rgba(0,0,0,0.55);
      z-index: 9999;
      display: flex;
      gap: 12px;
      animation: slideInRight 0.35s cubic-bezier(0.34,1.56,0.64,1);
      overflow: hidden;
    }
    @keyframes slideInRight {
      from { opacity: 0; transform: translateX(60px); }
      to   { opacity: 1; transform: translateX(0); }
    }
    .fp-toast-icon {
      width: 38px; height: 38px; flex-shrink: 0;
      background: rgba(0,201,167,0.15);
      border-radius: 10px;
      display: flex; align-items: center; justify-content: center;
      color: #00C9A7;
    }
    .fp-toast-icon .material-symbols-rounded { font-size: 20px; }
    .fp-toast-body { flex: 1; }
    .fp-toast-title {
      font-size: 0.875rem; font-weight: 700;
      color: #F0F4FF; margin-bottom: 3px;
    }
    .fp-toast-sub {
      font-size: 0.78rem; color: #94A3B8; line-height: 1.4; margin-bottom: 10px;
    }
    .fp-toast-cta {
      display: inline-block;
      background: #00C9A7;
      color: #0D1117;
      border: none; border-radius: 8px;
      padding: 6px 14px;
      font-size: 0.78rem; font-weight: 700;
      cursor: pointer; font-family: inherit;
      transition: background 0.2s;
    }
    .fp-toast-cta:hover { background: #00A88C; }
    .fp-toast-close {
      background: none; border: none;
      color: #4B5563; cursor: pointer;
      padding: 0; line-height: 1; align-self: flex-start;
      transition: color 0.2s;
    }
    .fp-toast-close:hover { color: #F0F4FF; }
    .fp-toast-close .material-symbols-rounded { font-size: 18px; }

    /* Progress bar countdown */
    .fp-toast-progress {
      position: absolute;
      bottom: 0; left: 0;
      height: 3px;
      background: linear-gradient(90deg, #00C9A7, #7B61FF);
      border-radius: 0 0 0 14px;
      animation: countdown linear forwards;
      width: 100%;
    }
    @keyframes countdown {
      from { width: 100%; }
      to   { width: 0%; }
    }
  `]
})
export class ForgotPasswordComponent {
  private fb    = inject(FormBuilder);
  private auth  = inject(AuthService);
  private router = inject(Router);

  form    = this.fb.group({ email: ['', [Validators.required, Validators.email]] });
  loading = signal(false);
  error   = signal('');
  showToast = signal(false);

  submit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    this.error.set('');

    this.auth.forgotPassword(this.form.value as any).subscribe({
      next: () => {
        this.loading.set(false);
        this.showToast.set(true);
        // Auto-redirect after 4 seconds (matches toast progress bar)
        setTimeout(() => this.goToReset(), 4000);
      },
      error: (err) => {
        this.error.set(err?.error?.message || 'Error sending reset link.');
        this.loading.set(false);
      }
    });
  }

  goToReset(): void {
    this.showToast.set(false);
    this.router.navigate(['/reset-password']);
  }
}
