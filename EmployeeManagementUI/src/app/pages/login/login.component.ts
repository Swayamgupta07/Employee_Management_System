import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  form = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  loading = signal(false);
  error = signal('');
  showPass = signal(false);

  ngAfterViewInit(): void {
    this.initGoogleSignIn();
  }

  private initGoogleSignIn(): void {
    const checkGoogle = setInterval(() => {
      const google = (window as any).google;
      if (google?.accounts?.id) {
        clearInterval(checkGoogle);
        google.accounts.id.initialize({
          client_id: '828872761587-1jgi45doh7a4kq8gkgmsqgpbk8r159if.apps.googleusercontent.com',
          callback: (res: any) => this.handleGoogleLogin(res.credential),
          use_fedcm: false // Disable FedCM to avoid AbortError being thrown on some localhost/browser setups
        });
      }
    }, 100);
  }

  private handleGoogleLogin(idToken: string): void {
    this.loading.set(true);
    this.error.set('');
    this.auth.googleLogin(idToken).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (err) => {
        this.error.set(err?.error?.message || 'Google Sign-In failed.');
        this.loading.set(false);
      },
      complete: () => this.loading.set(false)
    });
  }

  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.loading.set(true);
    this.error.set('');

    this.auth.login(this.form.value as any).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (err) => {
        const msg = err?.error?.message || '';
        if (msg.toLowerCase().includes('verify your email')) {
           // Extract email if possible or just rely on user knowing it. 
           // For better UX, we'd need email in LoginDto or the error response.
           // For now, if they used email as username, we use that.
           const identifier = this.form.value.username || '';
           if (identifier) {
             localStorage.setItem('pending_verify_email', identifier);
           }
           this.router.navigate(['/verify-otp']);
        } else {
           this.error.set(msg || 'Invalid credentials. Please try again.');
        }
        this.loading.set(false);
      },
      complete: () => this.loading.set(false)
    });
  }

  loginWithGoogle(): void {
    const google = (window as any).google;
    if (google?.accounts?.id) {
      try {
        google.accounts.id.prompt((notification: any) => {
          if (notification.isNotDisplayed()) {
            console.warn('Google One Tap not displayed:', notification.getNotDisplayedReason());
            // Fallback: If One Tap is blocked/already shown, just show a message.
            this.error.set('Google Sign-In prompt is already active or blocked by the browser. Please check your address bar or refresh.');
          }
          if (notification.isSkippedMoment()) {
            console.warn('Google One Tap skipped:', notification.getSkippedReason());
          }
        });
      } catch (e) {
        console.error('GSI Prompt Error:', e);
        this.error.set('Could not initialize Google prompt. Please try again.');
      }
    } else {
      this.error.set('Google Login is still initializing. Please wait a moment.');
    }
  }
}
