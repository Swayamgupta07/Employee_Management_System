namespace EmployeeManagementAPI.Models.DTO.Employee
{
    public class SearchEmployeeRequestDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? JoinedAfter { get; set; }
        public string? SearchTerm { get; internal set; }
    }
}
