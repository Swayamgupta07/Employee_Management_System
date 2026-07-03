import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { roleGuard } from './guards/role.guard';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';

export const routes: Routes = [
  // Public & Auth Routes
  { 
    path: 'login', 
    loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent) 
  },
  { 
    path: 'register', 
    loadComponent: () => import('./pages/register/register.component').then(m => m.RegisterComponent) 
  },
  { 
    path: 'forgot-password', 
    loadComponent: () => import('./pages/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent) 
  },
  { 
    path: 'reset-password', 
    loadComponent: () => import('./pages/reset-password/reset-password.component').then(m => m.ResetPasswordComponent) 
  },
  { 
    path: 'verify-otp', 
    loadComponent: () => import('./pages/otp-verify/otp-verify.component').then(m => m.OtpVerifyComponent) 
  },
  { 
    path: 'unauthorized', 
    loadComponent: () => import('./pages/unauthorized/unauthorized.component').then(m => m.UnauthorizedComponent) 
  },

  // Protected Layout-Nested Routes
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { 
        path: 'dashboard', 
        loadComponent: () => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent) 
      },
      { 
        path: 'employees', 
        loadComponent: () => import('./pages/employees/employees-list.component').then(m => m.EmployeesListComponent) 
      },
      { 
        path: 'departments', 
        loadComponent: () => import('./pages/departments/departments-list.component').then(m => m.DepartmentsListComponent) 
      },
      { 
        path: 'attendance', 
        loadComponent: () => import('./pages/attendance/attendance-list.component').then(m => m.AttendanceListComponent) 
      },
      {
        path: 'analytics',
        loadComponent: () => import('./pages/analytics/analytics.component').then(m => m.AnalyticsComponent),
        canActivate: [roleGuard],
        data: { roles: ['Admin', 'HR'] }
      },
      {
        path: 'reports',
        loadComponent: () => import('./pages/reports/reports.component').then(m => m.ReportsComponent),
        canActivate: [roleGuard],
        data: { roles: ['Admin', 'HR'] }
      },
      {
        path: 'approvals',
        loadComponent: () => import('./pages/approvals/approvals').then(m => m.ApprovalsComponent),
        canActivate: [roleGuard],
        data: { roles: ['Admin'] }
      }
    ]
  },

  // Wildcard Route
  { path: '**', redirectTo: 'dashboard' }
];
