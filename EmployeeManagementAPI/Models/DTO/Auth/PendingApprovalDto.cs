using System.Collections.Generic;

namespace EmployeeManagementAPI.Models.DTO.Auth
{
    public class PendingApprovalDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string RequestedRole { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
