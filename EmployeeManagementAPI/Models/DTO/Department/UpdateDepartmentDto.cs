namespace EmployeeManagementAPI.Models.DTO.Department
{
    public class UpdateDepartmentDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; internal set; }
    }

}
