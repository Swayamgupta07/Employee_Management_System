namespace EmployeeManagementAPI.Models.DTO.BonusFeature
{
    public class PerformanceMetricsDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal AttendanceScore { get; set; }
        public decimal ProjectScore { get; set; }
        public decimal OverallScore { get; set; }
    }
}
