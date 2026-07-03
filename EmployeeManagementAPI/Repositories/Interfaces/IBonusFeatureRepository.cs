using EmployeeManagementAPI.Models.DTO;
using EmployeeManagementAPI.Models.DTO.BonusFeature;
using System.Threading.Tasks;

namespace EmployeeManagementAPI.Repositories.Interfaces
{
    public interface IBonusFeatureRepository
    {
        Task<IEnumerable<HiringTrendDto>> GetHiringTrendAsync(DateTime start, DateTime end, CancellationToken cancellationToken);
        Task<IEnumerable<DepartmentGrowthDto>> GetDepartmentGrowthAsync(DateTime start, DateTime end, CancellationToken cancellationToken);
        Task<IEnumerable<AttendancePatternDto>> GetAttendancePatternsAsync(DateTime start, DateTime end, CancellationToken cancellationToken);
        Task<IEnumerable<PerformanceMetricsDto>> GetPerformanceMetricsAsync(DateTime start, DateTime end, CancellationToken cancellationToken);
    }
}
