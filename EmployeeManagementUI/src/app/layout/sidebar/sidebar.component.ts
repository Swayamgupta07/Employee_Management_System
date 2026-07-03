import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/auth.service';

interface NavItem {
  label: string;
  icon: string;
  route: string;
  roles?: string[];  // if set, only these roles see this item
  badge?: () => boolean;
}

interface NavGroup {
  label: string;
  roles?: string[]; // if set, entire group is restricted
  items: NavItem[];
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  auth = inject(AuthService);

  navGroups: NavGroup[] = [
    // ── Visible to EVERYONE (Employee + Admin) ─────────────────────────
    {
      label: 'Overview',
      items: [
        { label: 'Dashboard',   icon: 'dashboard',      route: '/dashboard' }
      ]
    },
    {
      label: 'HR Operations',
      items: [
        { label: 'Employees',   icon: 'group',          route: '/employees' },
        { label: 'Departments', icon: 'corporate_fare', route: '/departments' },
        { label: 'Attendance',  icon: 'calendar_month', route: '/attendance' },
      ]
    },

    // ── Admin / HR only ────────────────────────────────────────────────
    {
      label: 'Management',
      roles: ['Admin', 'HR'],
      items: [
        { label: 'Analytics',   icon: 'query_stats',   route: '/analytics' },
        { label: 'Reports',     icon: 'description',   route: '/reports'   },
      ]
    },
    {
      label: 'Admin',
      roles: ['Admin'],
      items: [
        { label: 'Role Approvals', icon: 'verified_user', route: '/approvals', badge: () => this.auth.pendingApprovalCount() > 0 }
      ]
    }
  ];

  /** Filter items inside a group by role */
  getVisibleItems(items: NavItem[]): NavItem[] {
    return items.filter(item => !item.roles || this.auth.hasRole(...item.roles));
  }

  /** Filter groups themselves — skip entire group if role not matched */
  getVisibleGroups(): NavGroup[] {
    return this.navGroups
      .filter(g => !g.roles || this.auth.hasRole(...g.roles))
      .map(g => ({ ...g, items: this.getVisibleItems(g.items) }))
      .filter(g => g.items.length > 0);
  }

  logout(): void {
    this.auth.logout();
  }
}
