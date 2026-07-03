namespace EmployeeManagementAPI.Models.DTO.Attendance
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public TimeSpan? WorkingHours { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
