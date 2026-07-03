export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  role?: string;
}

export interface AuthResponse {
  token: string;
  username: string;
  email: string;
  role: string;
  profilePictureUrl?: string;
  expires: string;
}

export interface User {
  username: string;
  email: string;
  role: string;
  profilePictureUrl?: string;
  token: string;
  expires: string;
}

export interface PasswordResetRequest {
  email: string;
}

export interface ResetPassword {
  token: string;
  newPassword: string;
}
