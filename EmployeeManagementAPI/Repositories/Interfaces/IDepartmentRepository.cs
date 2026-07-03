using EmployeeManagementAPI.Models.Data;
using System.Threading.Tasks;

namespace EmployeeManagementAPI.Repositories.Interfaces
{
    // Repositories/Interfaces/IDepartmentRepository.cs
    using EmployeeManagementAPI.Models.Data;
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<bool> DepartmentNameExistsAsync(string name, int? excludeId = null);
        Task<int> GetEmployeeCountByDepartmentAsync(int departmentId);
        Task<bool> ExistsAsync(int DepartmentId, CancellationToken cancellationToken);

    }

}
