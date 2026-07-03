using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Models.DTO.Auth
{
    public class GoogleAuthDto
    {
        [Required]
        public string IdToken { get; set; }
    }
}
