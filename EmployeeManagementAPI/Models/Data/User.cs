using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Models.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } 

        public bool IsActive { get; set; } = false;

        public bool IsEmailVerified { get; set; } = false;

        [MaxLength(6)]
        public string? OtpCode { get; set; }

        public DateTime? OtpExpiry { get; set; }

        public int FailedLoginAttempts { get; set; } = 0;

        public DateTime? LockoutEnd { get; set; }

        [MaxLength(20)]
        public string? RequestedRole { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? ProfilePictureUrl { get; set; }
    }
}
