using AutoMapper;
using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Models.DTO.Attendance;
using EmployeeManagementAPI.Repositories.Interfaces;
using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AttendanceController> _logger;

        public AttendanceController(
            IAttendanceRepository attendanceRepository,
            IMapper mapper,
            ILogger<AttendanceController> logger)
        {
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AttendanceDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var attendanceRecords = await _attendanceRepository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<AttendanceDto>>(attendanceRecords);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all attendance records");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AttendanceDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var attendance = await _attendanceRepository.GetByIdAsync(id);
                if (attendance == null)
                    return NotFound("Attendance record not found");

                var dto = _mapper.Map<AttendanceDto>(attendance);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving attendance record {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("employee/{employeeId}")]
        [ProducesResponseType(typeof(IEnumerable<AttendanceDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByEmployee(int employeeId, CancellationToken cancellationToken)
        {
            try
            {
                var attendanceList = await _attendanceRepository.GetAttendanceByEmployeeAsync(employeeId);
                if (attendanceList==null || !attendanceList.Any())
                    return NotFound("No attendance records found for employee");

                var dtos = _mapper.Map<IEnumerable<AttendanceDto>>(attendanceList);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving attendance for employee {employeeId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("daterange")]
        [ProducesResponseType(typeof(IEnumerable<AttendanceDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end, CancellationToken cancellationToken)
        {
            if (start > end)
                return BadRequest("Start date must be before end date");

            try
            {
                var attendanceList = await _attendanceRepository.GetAttendanceByDateRangeAsync(start, end);
                if (attendanceList== null ||!attendanceList.Any())
                    return NotFound("No attendance records found in the given date range");

                var dtos = _mapper.Map<IEnumerable<AttendanceDto>>(attendanceList);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving attendance between {start} and {end}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("monthlyreport")]
        [ProducesResponseType(typeof(IEnumerable<AttendanceDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetMonthlyReport([FromQuery] int month, [FromQuery] int year, CancellationToken cancellationToken)
        {
            if (month < 1 || month > 12)
                return BadRequest("Invalid month value");

            if (year < 1)
                return BadRequest("Invalid year value");

            try
            {
                var attendanceList = await _attendanceRepository.GetMonthlyAttendanceReportAsync(month, year);
                if (attendanceList == null || !attendanceList.Any())
                    return NotFound("No attendance records found for given month");

                var dtos = _mapper.Map<IEnumerable<AttendanceDto>>(attendanceList);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving monthly attendance report {month}/{year}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAttendanceDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input", errors = ModelState });

            try
            {
                var entity = _mapper.Map<Attendance>(dto);
                entity.Employee = null;
                await _attendanceRepository.CreateAsync(entity);
                var resultDto = _mapper.Map<AttendanceDto>(entity);
                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resultDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating attendance record");
                return StatusCode(500, new { message = ex.InnerException?.Message ?? ex.Message });
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAttendanceDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid input");

            if (id != dto.Id)
                return BadRequest("ID mismatch");

            try
            {
                var existing = await _attendanceRepository.GetByIdAsync(id);
                if (existing == null)
                    return NotFound("Attendance not found");

                _mapper.Map(dto, existing);
                await _attendanceRepository.UpdateAsync(existing);
                var resultDto = _mapper.Map<AttendanceDto>(existing);
            return Ok(resultDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating attendance record {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                var existing = await _attendanceRepository.GetByIdAsync(id);
                if (existing == null)
                    return NotFound("Attendance not found");

                await _attendanceRepository.DeleteAsync(id);
                return Ok("Attendance deleted Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting attendance record {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
