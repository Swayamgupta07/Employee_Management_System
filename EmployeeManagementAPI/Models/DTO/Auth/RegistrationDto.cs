using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Models.DTO.Auth
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public string? RequestedRole { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
