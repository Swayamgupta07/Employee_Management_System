import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { AuthService } from '../../services/auth.service';

interface PendingApproval {
  id: number;
  username: string;
  email: string;
  requestedRole: string;
  createdAt: string;
}

@Component({
  selector: 'app-approvals',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './approvals.html',
  styleUrl: './approvals.css'
})
export class ApprovalsComponent implements OnInit {
  private authService = inject(AuthService);

  approvals = signal<PendingApproval[]>([]);
  loading = signal(true);
  error = signal('');
  actionLoading = signal<number | null>(null);

  ngOnInit(): void {
    this.loadApprovals();
  }

  loadApprovals(): void {
    this.loading.set(true);
    this.error.set('');
    
    this.authService.getPendingApprovals().subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.approvals.set(res.data);
        }
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(err?.error?.message || 'Failed to load pending approvals');
        this.loading.set(false);
      }
    });
  }

  approve(id: number, username: string): void {
    if (!confirm(`Are you sure you want to approve ${username} as an Admin? They will be granted full system access.`)) return;
    
    this.actionLoading.set(id);
    this.authService.approveAdmin(id).subscribe({
      next: () => {
        this.approvals.update(list => list.filter(a => a.id !== id));
        this.actionLoading.set(null);
        // Optionally show a toast notification here
      },
      error: (err) => {
        alert(err?.error?.message || 'Failed to approve user');
        this.actionLoading.set(null);
      }
    });
  }

  reject(id: number, username: string): void {
    if (!confirm(`Are you sure you want to REJECT ${username}'s Admin request? Their pending account will be permanently deleted.`)) return;
    
    this.actionLoading.set(id);
    this.authService.rejectAdmin(id).subscribe({
      next: () => {
        this.approvals.update(list => list.filter(a => a.id !== id));
        this.actionLoading.set(null);
      },
      error: (err) => {
        alert(err?.error?.message || 'Failed to reject user');
        this.actionLoading.set(null);
      }
    });
  }
}
