using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Models.DTO.Auth
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
