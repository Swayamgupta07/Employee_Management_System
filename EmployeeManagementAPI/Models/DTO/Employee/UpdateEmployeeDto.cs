namespace EmployeeManagementAPI.Models.DTO.Employee
{
    public class UpdateEmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
