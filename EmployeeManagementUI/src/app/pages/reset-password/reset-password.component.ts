import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <div class="auth-page">
      <div class="auth-card">
        <div class="auth-logo">
          <div class="logo-icon">🔑</div>
          <h2>Reset Password</h2>
          <p>Paste the token from your email and choose a new password</p>
        </div>

        <!-- Success state -->
        <div *ngIf="success()" class="rp-success">
          <span class="material-symbols-rounded rp-success-icon">check_circle</span>
          <div class="rp-success-title">Password updated!</div>
          <div class="rp-success-sub">Redirecting to login in {{ countdown() }}s…</div>
          <a routerLink="/login" class="auth-btn" style="text-align:center;text-decoration:none;display:block;margin-top:16px">
            Go to Login now
          </a>
        </div>

        <!-- Form -->
        <form *ngIf="!success()" [formGroup]="form" (ngSubmit)="submit()">
          <div *ngIf="error()" class="alert alert-danger mb-4">{{ error() }}</div>

          <div class="mb-4">
            <label class="form-label">Reset Token</label>
            <textarea
              formControlName="token"
              class="form-control"
              rows="3"
              placeholder="Paste the full token from your email here…"
              style="resize:none;font-size:0.75rem;font-family:monospace;letter-spacing:0.02em;">
            </textarea>
            <div class="form-hint">Check your inbox for the token sent by the system</div>
          </div>

          <div class="mb-4">
            <label class="form-label">New Password</label>
            <div class="pw-wrap">
              <input
                formControlName="newPassword"
                [type]="showPw() ? 'text' : 'password'"
                class="form-control"
                placeholder="At least 6 characters">
              <button type="button" class="pw-toggle" (click)="showPw.set(!showPw())">
                <span class="material-symbols-rounded" style="font-size:18px">
                  {{ showPw() ? 'visibility_off' : 'visibility' }}
                </span>
              </button>
            </div>
          </div>

          <button type="submit" class="auth-btn" [disabled]="loading() || form.invalid">
            <span *ngIf="!loading()">Reset Password</span>
            <span *ngIf="loading()">Resetting…</span>
          </button>
        </form>

        <p class="auth-link"><a routerLink="/login">Back to Login</a></p>
      </div>
    </div>
  `,
  styles: [`
    .rp-success {
      text-align: center;
      padding: 24px 0 8px;
    }
    .rp-success-icon {
      font-size: 52px;
      color: #00C9A7;
      display: block;
      margin-bottom: 12px;
    }
    .rp-success-title {
      font-size: 1.1rem; font-weight: 700; color: #F0F4FF; margin-bottom: 6px;
    }
    .rp-success-sub {
      font-size: 0.82rem; color: #94A3B8; margin-bottom: 4px;
    }
    .form-hint {
      font-size: 0.72rem;
      color: #4B5563;
      margin-top: 6px;
    }
    .pw-wrap {
      position: relative;
    }
    .pw-toggle {
      position: absolute;
      right: 12px; top: 50%;
      transform: translateY(-50%);
      background: none; border: none;
      color: #4B5563; cursor: pointer;
      padding: 0; line-height: 1;
    }
    .pw-toggle:hover { color: #94A3B8; }
  `]
})
export class ResetPasswordComponent {
  private fb     = inject(FormBuilder);
  private auth   = inject(AuthService);
  private router = inject(Router);

  form = this.fb.group({
    token:       ['', Validators.required],
    newPassword: ['', [Validators.required, Validators.minLength(6)]]
  });

  loading  = signal(false);
  error    = signal('');
  success  = signal(false);
  showPw   = signal(false);
  countdown = signal(5);

  submit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    this.error.set('');

    this.auth.resetPassword(this.form.value as any).subscribe({
      next: () => {
        this.loading.set(false);
        this.success.set(true);
        // Countdown then redirect
        const interval = setInterval(() => {
          this.countdown.update(v => v - 1);
          if (this.countdown() <= 0) {
            clearInterval(interval);
            this.router.navigate(['/login']);
          }
        }, 1000);
      },
      error: (err) => {
        this.error.set(err?.error?.message || 'Reset failed. Check your token and try again.');
        this.loading.set(false);
      }
    });
  }
}
