using AutoMapper;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Models.DTO.Employee;
using EmployeeManagementAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper,
            ILogger<EmployeesController> logger)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<EmployeeDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponse<EmployeeDto>>> GetEmployees(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching employees page {PageNumber} with size {PageSize}", pageNumber, pageSize);

                var employees = await _employeeRepository.GetPagedAsync(pageNumber, pageSize, cancellationToken);
                var totalRecords = await _employeeRepository.CountAsync(cancellationToken);
                var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

                var response = new PagedResponse<EmployeeDto> 
                {
                    Success = true,
                    Message = "Employees retrieved successfully",
                    Data = employeeDtos,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords
                };

                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Employee retrieval cancelled");
                return StatusCode(499, ApiResponseFactory.Fail("Request cancelled"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employees");
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<EmployeeDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> GetEmployee(int id, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);

                if(employee == null)
                    return NotFound(ApiResponseFactory.Fail($"Employee with ID {id} not found"));

                var dto = _mapper.Map<EmployeeDto>(employee);
                return Ok(ApiResponseFactory.Success(dto, "Employee retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee {EmployeeId}", id);
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<EmployeeDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeDto>>>> SearchEmployees(
            [FromQuery] SearchEmployeeRequestDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(request.Name) && string.IsNullOrWhiteSpace(request.Email)
                   && !request.DepartmentId.HasValue && !request.JoinedAfter.HasValue)
                {
                    return BadRequest(ApiResponseFactory.Fail("At least one search parameter must be provided"));
                }

                var employees = await _employeeRepository.SearchAsync(request, cancellationToken);
                var dtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

                return Ok(ApiResponseFactory.Success(dtos, "Search results"));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error during employee search");
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> CreateEmployee(
            [FromBody] CreateEmployeeDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseFactory.Fail("Invalid input"));

            try
            {
                if(await _employeeRepository.EmailExistAsync(dto.Email, cancellationToken))
                    return Conflict(ApiResponseFactory.Fail("Email already exists"));

                if(!await _departmentRepository.ExistsAsync(dto.DepartmentId, cancellationToken))
                    return BadRequest(ApiResponseFactory.Fail("Invalid department"));

                var employee = _mapper.Map<Employee>(dto);

                await _employeeRepository.CreateAsync(employee, cancellationToken);

                var createdDto = _mapper.Map<EmployeeDto>(employee);

                _logger.LogInformation("Employee created by {User}", User?.Identity?.Name ?? "Unknown");

                return CreatedAtAction(nameof(GetEmployee), new { id = createdDto.Id },
                    ApiResponseFactory.Success(createdDto, "Employee created successfully"));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                _logger.LogError(ex, "Exception details: {StackTrace}", ex.StackTrace);
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> UpdateEmployee(
            int id, [FromBody] UpdateEmployeeDto dto, CancellationToken cancellationToken = default)
        {
            if(!ModelState.IsValid)
                return BadRequest(ApiResponseFactory.Fail("Invalid input"));

            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
                if(employee == null)
                    return NotFound(ApiResponseFactory.Fail("Employee not found"));

                if(await _employeeRepository.EmailExistsAsync(dto.Email, id, cancellationToken))
                    return Conflict(ApiResponseFactory.Fail("Email already exists"));

                if(!await _departmentRepository.ExistsAsync(dto.DepartmentId, cancellationToken))
                    return BadRequest(ApiResponseFactory.Fail("Invalid department"));

                _mapper.Map(dto, employee);

                await _employeeRepository.UpdateAsync(employee, cancellationToken);

                _logger.LogInformation("Employee updated by {User} - {EmployeeId}", User?.Identity?.Name, id);

                var updatedDto = _mapper.Map<EmployeeDto>(employee);

                return Ok(ApiResponseFactory.Success(updatedDto, "Employee updated successfully"));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating employee {EmployeeId}", id);
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,HR")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> DeleteEmployee(
            int id, CancellationToken cancellationToken = default)
        {
            try
            {
                if(id <= 0)
                    return BadRequest(ApiResponseFactory.Fail("Invalid Employee ID"));

                var exists = await _employeeRepository.ExistsAsync(id, cancellationToken);
                if(!exists)
                    return NotFound(ApiResponseFactory.Fail("Employee not found"));

                await _employeeRepository.DeleteAsync(id, cancellationToken);

                var userName = User?.Identity?.Name ?? "Unknown";

                _logger.LogInformation("Employee deleted by {User} - {EmployeeId}", userName, id);

                return Ok(ApiResponseFactory.Success($"Employee {id} deleted successfully"));
            }
            catch(OperationCanceledException)
            {
                _logger.LogWarning("Delete employee operation cancelled");
                return StatusCode(499, ApiResponseFactory.Fail("Request cancelled"));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee {EmployeeId}", id);
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }

        [HttpPatch("deactivate/{id}")]
        [Authorize(Roles = "Admin,HR")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> DeactivateEmployee(
            int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
                if(employee == null)
                    return NotFound(ApiResponseFactory.Fail("Employee not found"));

                employee.IsActive = false;
                await _employeeRepository.UpdateAsync(employee, cancellationToken);

                _logger.LogInformation("Employee deactivated by {User} - {EmployeeId}", User?.Identity?.Name, id);

                return Ok(ApiResponseFactory.Success($"Employee {id} deactivated successfully"));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deactivating employee {EmployeeId}", id);
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }
    }
}