export interface Attendance {
  id: number;
  employeeId: number;
  employeeName?: string;
  date: string;
  checkInTime: string;   // matches API: CheckInTime (TimeSpan → "HH:mm:ss")
  checkOutTime: string;  // matches API: CheckOutTime
  workingHours?: string;
  status: string;
  notes?: string;
}

export interface CreateAttendance {
  employeeId: number;
  date: string;
  checkInTime: string;
  checkOutTime?: string | null;
  status: string;
  notes?: string;
}

export interface UpdateAttendance {
  id: number;
  employeeId: number;
  date: string;
  checkInTime: string;
  checkOutTime?: string | null;
  status: string;
  notes?: string;
}
