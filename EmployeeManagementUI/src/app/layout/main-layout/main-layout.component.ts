import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { HeaderComponent } from '../header/header.component';
import { ProfilePictureModalComponent } from '../../components/profile-picture-modal/profile-picture-modal';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, SidebarComponent, HeaderComponent, ProfilePictureModalComponent],
  template: `
    <div class="app-shell">
      <app-sidebar></app-sidebar>
      <div class="main-content">
        <app-header></app-header>
        <main class="page-content">
          <router-outlet></router-outlet>
        </main>
      </div>
      @if (auth.showProfileModal()) {
        <app-profile-picture-modal (closed)="auth.showProfileModal.set(false)"></app-profile-picture-modal>
      }
    </div>
  `
})
export class MainLayoutComponent {
  constructor(public auth: AuthService) {}
}
