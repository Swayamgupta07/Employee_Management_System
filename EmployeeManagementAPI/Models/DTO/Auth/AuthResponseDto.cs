namespace EmployeeManagementAPI.Models.DTO.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime Expires { get; set; }
    }
}
