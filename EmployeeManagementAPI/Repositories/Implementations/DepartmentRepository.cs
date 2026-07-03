using Microsoft.EntityFrameworkCore;
using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Repositories.Interfaces;

namespace EmployeeManagementAPI.Repositories.Implementations
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DepartmentRepository> _logger;

        public DepartmentRepository(ApplicationDbContext context, ILogger<DepartmentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        private async Task<T> ExecuteSafeAsync<T>(Func<Task<T>> action, string operation)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while {Operation}", operation);
                throw;
            }
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                return await _context.Departments
                    .Include(d => d.Employees)
                    .AsNoTracking()
                    .OrderBy(d => d.Name)
                    .ToListAsync();
            }, "fetching all departments");
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                return await _context.Departments
                    .Include(d => d.Employees)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.Id == id);
            }, $"fetching department by id {id}");
        }

        public async Task CreateAsync(Department department)
        {
            await ExecuteSafeAsync(async () =>
            {
                department.CreatedAt = DateTime.UtcNow;
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return true;
            }, $"creating department {department.Name}");
        }

        public async Task UpdateAsync(Department department)
        {
            await ExecuteSafeAsync(async () =>
            {
                _context.Entry(department).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }, $"updating department {department.Id}");
        }

        public async Task DeleteAsync(int id)
        {
            await ExecuteSafeAsync(async () =>
            {
                var department = await _context.Departments.FindAsync(id);
                if(department != null)
                {
                    var hasEmployees = await _context.Employees.AnyAsync(e => e.DepartmentId == id && e.IsActive);
                    if(!hasEmployees)
                    {
                        _context.Departments.Remove(department);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new InvalidOperationException("Cannot delete department with active employees");
                    }
                }
                return true;
            }, $"deleting department {id}");
        }

        public async Task<bool> DepartmentNameExistsAsync(string name, int? excludeId = null)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var query = _context.Departments.Where(d => d.Name.ToLower() == name.ToLower());
                if (excludeId.HasValue)
                    query = query.Where(d => d.Id != excludeId.Value);
                return await query.AnyAsync();
            }, $"checking if department name {name} exists");
        }

        public async Task<int> GetEmployeeCountByDepartmentAsync(int departmentId)
        {
            return await ExecuteSafeAsync(async () =>
            {
                return await _context.Employees.CountAsync(e => e.DepartmentId == departmentId && e.IsActive);
            }, $"getting employee count for department {departmentId}");
        }

        public async Task<bool> ExistsAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Departments.AnyAsync(d => d.Id == departmentId, cancellationToken);
        }
    }
}
