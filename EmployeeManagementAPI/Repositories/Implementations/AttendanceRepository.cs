using Microsoft.EntityFrameworkCore;
using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Repositories.Interfaces;

namespace EmployeeManagementAPI.Repositories.Implementations
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AttendanceRepository> _logger;

        public AttendanceRepository(ApplicationDbContext context, ILogger<AttendanceRepository> logger)
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

        public async Task<IEnumerable<Attendance>> GetAttendanceByEmployeeAsync(int employeeId)
        {
            return await ExecuteSafeAsync(async () =>
                await _context.Attendances
                    .Include(a => a.Employee)
                    .Where(a => a.EmployeeId == employeeId)
                    .AsNoTracking()
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(),
                $"fetching attendance for employee {employeeId}");
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await ExecuteSafeAsync(async () =>
                await _context.Attendances
                    .Include(a => a.Employee)
                    .Where(a => a.Date >= startDate && a.Date <= endDate)
                    .AsNoTracking()
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(),
                $"fetching attendance from {startDate} to {endDate}");
        }

        public async Task<IEnumerable<Attendance>> GetMonthlyAttendanceReportAsync(int month, int year)
        {
            return await ExecuteSafeAsync(async () =>
                await _context.Attendances
                    .Include(a => a.Employee)
                    .Where(a => a.Date.Month == month && a.Date.Year == year)
                    .AsNoTracking()
                    .OrderBy(a => a.Date)
                    .ToListAsync(),
                $"fetching monthly attendance report for {month}/{year}");
        }

        public async Task CreateAsync(Attendance entity)
        {
            await ExecuteSafeAsync(async () =>
            {
                _context.Attendances.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }, "creating attendance");
        }

        public async Task UpdateAsync(Attendance entity)
        {
            await ExecuteSafeAsync(async () =>
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }, $"updating attendance {entity.Id}");
        }

        public async Task DeleteAsync(int id)
        {
            await ExecuteSafeAsync(async () =>
            {
                var attendance = await _context.Attendances.FindAsync(id);
                if (attendance != null)
                {
                    _context.Attendances.Remove(attendance);
                    await _context.SaveChangesAsync();
                }
                return true;
            }, $"deleting attendance {id}");
        }

        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
                await _context.Attendances
                    .Include(a => a.Employee)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == id),
                $"fetching attendance by id {id}");
        }

        public async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await ExecuteSafeAsync(async () =>
                await _context.Attendances
                    .Include(a => a.Employee)
                    .AsNoTracking()
                    .OrderByDescending(a => a.Date)
                    .ToListAsync(),
                "fetching all attendance");
        }
    }
}
