using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Models.DTO.Department
{
    public class CreateDepartmentDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
