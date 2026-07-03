using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Models.DTO.Employee
{
    public class CreateEmployeeDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }

        [Required]
        public DateTime HireDate { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
