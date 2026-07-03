namespace EmployeeManagementAPI.Models.DTO.BonusFeature
{
    public class AttendancePatternDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int LateArrivals { get; set; }
    }
}
