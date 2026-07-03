using DocumentFormat.OpenXml.InkML;
using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Models.DTO.Employee;
using EmployeeManagementAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(ApplicationDbContext context, ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        private IQueryable<Employee> BaseQuery(bool includeDepartment = true, bool onlyActive = true)
        {
            var query = _context.Employees.AsQueryable();
            if (includeDepartment)
                query = query.Include(e => e.Department);
            if (onlyActive)
                query = query.Where(e => e.IsActive);
            return query;
        }

        public async Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => string.Equals(e.Email, email, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Where(e => e.DepartmentId == departmentId && e.IsActive)
                .Include(e => e.Department)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Employees.AsQueryable();
            if (excludeId.HasValue)
                query = query.Where(e => e.Id != excludeId.Value);
            return await query.AnyAsync(e => string.Equals(e.Email, email, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }

        public async Task<IEnumerable<Employee>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.FirstName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(e => e.Department)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id && e.IsActive, cancellationToken);
        }

        public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id && e.IsActive, cancellationToken);
        }

        public async Task CreateAsync(Employee entity, CancellationToken cancellationToken = default)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = true;
            _context.Employees.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Employee entity, CancellationToken cancellationToken = default)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Employees.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var employee = await _context.Employees.FindAsync(new object[] { id }, cancellationToken);
            if (employee != null)
            {
                employee.IsActive = false;
                employee.UpdatedAt = DateTime.UtcNow;
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Employees.CountAsync(e => e.IsActive, cancellationToken);
        }

        public async Task<IEnumerable<Employee>> SearchAsync(SearchEmployeeRequestDto request, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Searching employees with filters: {@Request}", request);
            var query = BaseQuery().AsNoTracking();
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                query = query.Where(e =>
                    e.FirstName.ToLower().Contains(term) ||
                    e.LastName.ToLower().Contains(term) ||
                    e.Email.ToLower().Contains(term) ||
                    e.Position.ToLower().Contains(term));
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                var email = request.Email.ToLower();
                query = query.Where(e => e.Email.ToLower().Contains(email));
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                var name = request.Name.ToLower();
                query = query.Where(e =>
                    e.FirstName.ToLower().Contains(name) ||
                    e.LastName.ToLower().Contains(name));
            }
            if (request.DepartmentId.HasValue)
            {
                query = query.Where(e => e.DepartmentId == request.DepartmentId.Value);
            }
            if (request.JoinedAfter.HasValue)
            {
                query = query.Where(e => e.HireDate >= request.JoinedAfter.Value);
            }
            query = query.OrderBy(e => e.FirstName);
            return await query.ToListAsync(cancellationToken);
        }
        public async Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken)
        {
            var loweredEmail = email.ToLower();
            return await _context.Employees.AnyAsync(e => e.Email.ToLower() == loweredEmail, cancellationToken);
        }

    }
}
