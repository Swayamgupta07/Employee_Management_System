using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Models.DTO.Attendance
{
    public class CreateAttendanceDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string? CheckInTime { get; set; }
        public string? CheckOutTime { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Present";

        public string? Notes { get; set; }
    }
}
