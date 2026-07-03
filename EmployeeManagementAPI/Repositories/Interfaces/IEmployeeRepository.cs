using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Models.DTO.Employee;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeManagementAPI.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Employee>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Employee>> SearchAsync(SearchEmployeeRequestDto request, CancellationToken cancellationToken = default);
        Task CreateAsync(Employee entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(Employee entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken);
    }
}
