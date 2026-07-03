using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models.DTO;
using EmployeeManagementAPI.Models.DTO.BonusFeature;
using EmployeeManagementAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeManagementAPI.Repositories.Implementations
{
    public class BonusFeatureRepository : IBonusFeatureRepository
    {
        private readonly ApplicationDbContext _context;

        public BonusFeatureRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HiringTrendDto>> GetHiringTrendAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Where(e => e.HireDate >= start && e.HireDate <= end)
                .GroupBy(e => new { e.HireDate.Year, e.HireDate.Month })
                .Select(g => new HiringTrendDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalHires = g.Count()
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<DepartmentGrowthDto>> GetDepartmentGrowthAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .Where(e => e.HireDate >= start && e.HireDate <= end)
                .Include(e => e.Department) // Ensure Department is included for navigation property
                .GroupBy(e => new { e.DepartmentId, e.Department.Name })
                .Select(g => new DepartmentGrowthDto
                {
                    DepartmentId = g.Key.DepartmentId,
                    DepartmentName = g.Key.Name,
                    EmployeeCount = g.Count(),
                    Date = DateTime.UtcNow
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<AttendancePatternDto>> GetAttendancePatternsAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default)
        {
            return await _context.Attendances
                .Where(a => a.Date >= start && a.Date <= end)
                .Include(a => a.Employee) // Load employee navigation properties
                .GroupBy(a => new { a.EmployeeId, a.Employee.FirstName, a.Employee.LastName })
                .Select(g => new AttendancePatternDto
                {
                    EmployeeId = g.Key.EmployeeId,
                    EmployeeName = $"{g.Key.FirstName} {g.Key.LastName}",
                    PresentDays = g.Count(a => a.Status == "Present"),
                    AbsentDays = g.Count(a => a.Status == "Absent"),
                    LateArrivals = g.Count(a => a.Status == "Late")
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PerformanceMetricsDto>> GetPerformanceMetricsAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default)
        {
            // Attendance scores grouped by employee ID
            var attendanceData = await _context.Attendances
                .Where(a => a.Date >= start && a.Date <= end)
                .GroupBy(a => a.EmployeeId)
                .Select(g => new
                {
                    EmployeeId = g.Key,
                    AttendanceScore = g.Count(a => a.Status == "Present") * 10
                })
                .ToListAsync(cancellationToken);

            var employeeIds = attendanceData.Select(a => a.EmployeeId).ToList();

            // Get employee names in a single query to prevent N+1 issue
            var employees = await _context.Employees
                .Where(e => employeeIds.Contains(e.Id))
                .Select(e => new { e.Id, FullName = e.FirstName + " " + e.LastName })
                .ToListAsync(cancellationToken);

            // Mapping attendance data and employees to performance metrics DTOs
            var performanceMetrics = attendanceData.Select(a =>
            {
                var emp = employees.FirstOrDefault(e => e.Id == a.EmployeeId);
                return new PerformanceMetricsDto
                {
                    EmployeeId = a.EmployeeId,
                    EmployeeName = emp?.FullName ?? "Unknown",
                    AttendanceScore = a.AttendanceScore,
                    ProjectScore = 50, // Placeholder static value; ideally fetch real project scores
                    OverallScore = a.AttendanceScore + 50
                };
            });

            return performanceMetrics;
        }
    }
}
