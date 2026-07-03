using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Models.DTO.Auth
{
    public class PasswordResetRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
