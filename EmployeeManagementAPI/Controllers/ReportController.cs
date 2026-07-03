using Microsoft.AspNetCore.Mvc;
using EmployeeManagementAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagementAPI.Models.Data;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IReportRepository reportRepository, ILogger<ReportsController> logger)
        {
            _reportRepository = reportRepository;
            _logger = logger;
        }

        [HttpGet("employees/excel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeesExcel(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Generating Excel report for employees.");

                var stream = await _reportRepository.GenerateEmployeeExcelReportStreamAsync(cancellationToken);

                if (stream == null || stream.Length == 0)
                {
                    _logger.LogWarning("No employee data available for Excel report.");
                    return NotFound(ApiResponseFactory.Fail("No employee data available for Excel report."));
                }

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");

            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Excel report generation cancelled.");
                return StatusCode(499, ApiResponseFactory.Fail("Request cancelled"));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error generating Excel report");
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }

        [HttpGet("employees/pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeesPdf(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Generating PDF report for employees.");

                var stream = await _reportRepository.GenerateEmployeePdfReportStreamAsync(cancellationToken);

                if (stream == null || stream.Length == 0)
                {
                    _logger.LogWarning("No employee data available for PDF report.");
                    return NotFound(ApiResponseFactory.Fail("No employee data available for PDF report."));
                }

                return File(stream, "application/pdf", "Employees.pdf");

            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("PDF report generation cancelled.");
                return StatusCode(499, ApiResponseFactory.Fail("Request cancelled"));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF report");
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }
    }
}
