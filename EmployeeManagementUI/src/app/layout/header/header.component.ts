import { Component, OnInit, inject, signal, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { EmployeeService } from '../../services/employee.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit {
  auth = inject(AuthService);
  router = inject(Router);
  private employeeSvc = inject(EmployeeService);

  showUserMenu = false;
  showHelpToast = signal(false);
  helpToastTimeout: any;

  get pendingApprovalCount() {
    return this.auth.pendingApprovalCount;
  }

  // Search state
  searchQuery = signal('');
  showSearchSuggestions = signal(false);

  get isAdmin(): boolean {
    return this.auth.hasRole('Admin');
  }

  ngOnInit(): void {
    if (this.isAdmin) {
      this.auth.getPendingApprovals().subscribe({
        next: (res) => {
          if (res.success && res.data) {
            this.auth.pendingApprovalCount.set(res.data.length);
          }
        },
        error: () => {} // Silent fail — badge just won't show
      });
    }
  }

  // ===== Search =====
  onSearchInput(event: any): void {
    const q = event.target.value.trim();
    this.searchQuery.set(q);
    this.showSearchSuggestions.set(q.length > 0);
  }

  onSearchEnter(): void {
    const q = this.searchQuery();
    if (q) {
      this.showSearchSuggestions.set(false);
      this.router.navigate(['/employees'], { queryParams: { search: q } });
    }
  }

  navigateSearch(path: string): void {
    this.showSearchSuggestions.set(false);
    this.router.navigate([path]);
  }

  closeSearch(): void {
    setTimeout(() => this.showSearchSuggestions.set(false), 150);
  }

  // ===== Help Toast =====
  openHelp(): void {
    this.showHelpToast.set(true);
    clearTimeout(this.helpToastTimeout);
    this.helpToastTimeout = setTimeout(() => this.showHelpToast.set(false), 10000);
  }

  closeHelp(): void {
    this.showHelpToast.set(false);
    clearTimeout(this.helpToastTimeout);
  }

  // ===== User Menu =====
  toggleUserMenu(): void { this.showUserMenu = !this.showUserMenu; }
  closeUserMenu(): void  { this.showUserMenu = false; }

  logout(): void {
    this.auth.logout();
    this.showUserMenu = false;
  }

  @HostListener('document:click')
  onDocumentClick(): void {
    this.showUserMenu = false;
  }

  get pageTitle(): string {
    const url = this.router.url;
    if (url.includes('dashboard'))  return 'Dashboard';
    if (url.includes('employees'))  return 'Employees';
    if (url.includes('departments'))return 'Departments';
    if (url.includes('attendance')) return 'Attendance';
    if (url.includes('analytics'))  return 'Analytics';
    if (url.includes('reports'))    return 'Reports';
    if (url.includes('approvals'))  return 'Role Approvals';
    return 'Employee Management';
  }

  get currentMonth(): string {
    return new Date().toLocaleDateString('en-US', { month: 'long', year: 'numeric' });
  }
}
