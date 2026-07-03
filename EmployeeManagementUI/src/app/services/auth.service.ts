import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  LoginRequest, RegisterRequest, AuthResponse,
  User, PasswordResetRequest, ResetPassword
} from '../models/auth.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly TOKEN_KEY = 'emp_token';
  private readonly USER_KEY = 'emp_user';
  private apiUrl = `${environment.apiUrl}/auth`;

  private _currentUser = signal<User | null>(this.loadUser());
  currentUser = this._currentUser.asReadonly();
  
  showProfileModal = signal(false);

  isLoggedIn = computed(() => !!this._currentUser());
  role = computed(() => this._currentUser()?.role ?? '');
  pendingApprovalCount = signal(0);

  constructor(private http: HttpClient, private router: Router) {}

  login(dto: LoginRequest): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.apiUrl}/login`, dto).pipe(
      tap(res => {
        if (res.success && res.data) {
          const user: User = {
            username: res.data.username,
            email: res.data.email,
            role: res.data.role,
            profilePictureUrl: this.getFullProfilePictureUrl(res.data.profilePictureUrl),
            token: res.data.token,
            expires: res.data.expires
          };
          localStorage.setItem(this.TOKEN_KEY, res.data.token);
          localStorage.setItem(this.USER_KEY, JSON.stringify(user));
          this._currentUser.set(user);
        }
      })
    );
  }

  register(dto: RegisterRequest): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.apiUrl}/register`, dto).pipe(
      tap(res => {
        if (res.success && res.data) {
          const user: User = {
            username: res.data.username,
            email: res.data.email,
            role: res.data.role,
            profilePictureUrl: this.getFullProfilePictureUrl(res.data.profilePictureUrl),
            token: res.data.token,
            expires: res.data.expires
          };
          localStorage.setItem(this.TOKEN_KEY, res.data.token);
          localStorage.setItem(this.USER_KEY, JSON.stringify(user));
          this._currentUser.set(user);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this._currentUser.set(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  forgotPassword(dto: PasswordResetRequest): Observable<ApiResponse<null>> {
    return this.http.post<ApiResponse<null>>(`${this.apiUrl}/password-reset-request`, dto);
  }

  resetPassword(dto: ResetPassword): Observable<ApiResponse<null>> {
    return this.http.post<ApiResponse<null>>(`${this.apiUrl}/reset-password`, dto);
  }

  getPendingApprovals(): Observable<ApiResponse<any[]>> {
    return this.http.get<ApiResponse<any[]>>(`${this.apiUrl}/pending-approvals`);
  }

  approveAdmin(id: number): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.apiUrl}/approve-admin/${id}`, {});
  }

  rejectAdmin(id: number): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.apiUrl}/reject-admin/${id}`, {});
  }

  hasRole(...roles: string[]): boolean {
    const userRole = this.role();
    return roles.some(r => r.toLowerCase() === userRole?.toLowerCase());
  }

  googleLogin(idToken: string): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.apiUrl}/google-login`, { idToken }).pipe(
      tap(res => {
        if (res.success && res.data) {
          const user: User = {
            username: res.data.username,
            email: res.data.email,
            role: res.data.role,
            profilePictureUrl: this.getFullProfilePictureUrl(res.data.profilePictureUrl),
            token: res.data.token,
            expires: res.data.expires
          };
          localStorage.setItem(this.TOKEN_KEY, res.data.token);
          localStorage.setItem(this.USER_KEY, JSON.stringify(user));
          this._currentUser.set(user);
        }
      })
    );
  }

  verifyOtp(email: string, otp: string): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.apiUrl}/verify-otp`, { email, otp }).pipe(
      tap(res => {
        if (res.success && res.data) {
          const user: User = {
            username: res.data.username,
            email: res.data.email,
            role: res.data.role,
            profilePictureUrl: this.getFullProfilePictureUrl(res.data.profilePictureUrl),
            token: res.data.token,
            expires: res.data.expires
          };
          localStorage.setItem(this.TOKEN_KEY, res.data.token);
          localStorage.setItem(this.USER_KEY, JSON.stringify(user));
          this._currentUser.set(user);
        }
      })
    );
  }

  uploadProfilePicture(base64Image: string): Observable<ApiResponse<{ profilePictureUrl: string }>> {
    return this.http.post<ApiResponse<{ profilePictureUrl: string }>>(`${this.apiUrl}/upload-dp`, { base64Image }).pipe(
      tap(res => {
        if (res.success && res.data) {
          const user = this._currentUser();
          if (user) {
            const updatedUser = { ...user, profilePictureUrl: this.getFullProfilePictureUrl(res.data.profilePictureUrl) || undefined };
            localStorage.setItem(this.USER_KEY, JSON.stringify(updatedUser));
            this._currentUser.set(updatedUser);
          }
        }
      })
    );
  }

  setDefaultProfilePicture(avatarId: 'male' | 'female'): Observable<ApiResponse<{ profilePictureUrl: string }>> {
    return this.http.post<ApiResponse<{ profilePictureUrl: string }>>(`${this.apiUrl}/set-default-dp`, { defaultAvatarId: avatarId }).pipe(
      tap(res => {
        if (res.success && res.data) {
          const user = this._currentUser();
          if (user) {
            const updatedUser = { ...user, profilePictureUrl: res.data.profilePictureUrl };
            localStorage.setItem(this.USER_KEY, JSON.stringify(updatedUser));
            this._currentUser.set(updatedUser);
          }
        }
      })
    );
  }

  private loadUser(): User | null {
    try {
      const raw = localStorage.getItem(this.USER_KEY);
      return raw ? JSON.parse(raw) : null;
    } catch {
      return null;
    }
  }

  private getFullProfilePictureUrl(url?: string | null): string | undefined {
    if (!url) return undefined;
    if (url.startsWith('data:') || url.startsWith('http')) return url;
    return environment.apiUrl.replace('/api', '') + url;
  }
}
