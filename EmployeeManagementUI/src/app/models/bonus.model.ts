export interface HiringTrend {
  period: string;
  hires: number;
  departmentName?: string;
}

export interface DepartmentGrowth {
  departmentName: string;
  startCount: number;
  endCount: number;
  growthPercent: number;
}

export interface AttendancePattern {
  period: string;
  presentCount: number;
  absentCount: number;
  lateCount: number;
  attendanceRate: number;
}

export interface PerformanceMetrics {
  departmentName: string;
  averageAttendanceRate: number;
  totalEmployees: number;
  activeEmployees: number;
}
