import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-unauthorized',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="auth-page">
      <div class="auth-card text-center pb-5">
        <h1 style="font-size: 5rem; line-height: 1; color: var(--accent);">403</h1>
        <h3 class="text-white mt-3 mb-2">Access Denied</h3>
        <p class="text-muted mb-4">You don't have permission to access this page.</p>
        <button class="btn btn-primary px-4" routerLink="/dashboard">Return to Dashboard</button>
      </div>
    </div>
  `
})
export class UnauthorizedComponent {}
