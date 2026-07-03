using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Models.DTO;
using EmployeeManagementAPI.Models.DTO.BonusFeature;
using EmployeeManagementAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BonusFeaturesController : ControllerBase
    {
        private readonly IBonusFeatureRepository _bonusRepository;
        private readonly ILogger<BonusFeaturesController> _logger;

        public BonusFeaturesController(IBonusFeatureRepository bonusRepository, ILogger<BonusFeaturesController> logger)
        {
            _bonusRepository = bonusRepository ?? throw new ArgumentNullException(nameof(bonusRepository));
            _logger = logger;
        }

        private bool IsValidDateRange(DateTime start, DateTime end)
        {
            return start <= end;
        }

        private async Task<IActionResult> ExecuteWithDateRangeAsync<T>(Func<CancellationToken, Task<IEnumerable<T>>> func,DateTime start, DateTime end, CancellationToken cancellationToken,string notFoundMessage = "No data found for the specified period.")
        {
            if (!IsValidDateRange(start, end))
                return BadRequest(ApiResponseFactory.Fail("Start date must be before end date."));
            try
            {
                var data = await func(cancellationToken);
                
                // Return Ok even if list is empty to avoid 404 console errors in Dashboard
                return Ok(ApiResponseFactory.Success(data ?? Enumerable.Empty<T>()));
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Operation cancelled for period {Start} to {End}", start, end);
                return StatusCode(499, ApiResponseFactory.Fail("Request cancelled"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred for period {Start} to {End}", start, end);
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }}


        [HttpGet("hiring-trends")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> GetHiringTrends([FromQuery] DateTime start, [FromQuery] DateTime end, CancellationToken ct) =>
            ExecuteWithDateRangeAsync<HiringTrendDto>(
                c => _bonusRepository.GetHiringTrendAsync(start, end, c), start, end, ct, "No hiring trends found for the specified period.");

        [HttpGet("department-growth")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> GetDepartmentGrowth([FromQuery] DateTime start, [FromQuery] DateTime end, CancellationToken ct) =>
            ExecuteWithDateRangeAsync<DepartmentGrowthDto>(
                c => _bonusRepository.GetDepartmentGrowthAsync(start, end, c), start, end, ct, "No department growth data found for the specified period.");


        [HttpGet("attendance-patterns")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> GetAttendancePatterns([FromQuery] DateTime start, [FromQuery] DateTime end, CancellationToken ct) =>
            ExecuteWithDateRangeAsync<AttendancePatternDto>(
                c => _bonusRepository.GetAttendancePatternsAsync(start, end, c), start, end, ct, "No attendance patterns found for the specified period.");


        [HttpGet("performance-metrics")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> GetPerformanceMetrics([FromQuery] DateTime start, [FromQuery] DateTime end, CancellationToken ct) =>
            ExecuteWithDateRangeAsync<PerformanceMetricsDto>(
                c => _bonusRepository.GetPerformanceMetricsAsync(start, end, c), start, end, ct, "No performance metrics found for the specified period.");

    }
}
