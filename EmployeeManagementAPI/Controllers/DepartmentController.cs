using AutoMapper;
using EmployeeManagementAPI.Models.Data;
using EmployeeManagementAPI.Models.DTO.Department;
using EmployeeManagementAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/departments")]
    [Authorize]
    
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(IDepartmentRepository departmentRepository, IMapper mapper, ILogger<DepartmentController> logger)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<DepartmentDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var departments = await _departmentRepository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
                return Ok(ApiResponseFactory.Success(dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all departments");
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<DepartmentDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(id);
                if (department == null)
                    return NotFound(ApiResponseFactory.Fail("Department not found"));

                var dto = _mapper.Map<DepartmentDto>(department);
                return Ok(ApiResponseFactory.Success(dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving department {id}");
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<DepartmentDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseFactory.Fail("Invalid input"));
            try
            {
                if (await _departmentRepository.DepartmentNameExistsAsync(dto.Name))
                    return Conflict(ApiResponseFactory.Fail("Department name already exists"));

                var department = _mapper.Map<Department>(dto);
                await _departmentRepository.CreateAsync(department);

                var createdDto = _mapper.Map<DepartmentDto>(department);
                return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, ApiResponseFactory.Success(createdDto, "Department created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating department");
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<DepartmentDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseFactory.Fail("Invalid input"));
            if (id != dto.Id)
                return BadRequest(ApiResponseFactory.Fail("ID mismatch"));

            try
            {
                var existing = await _departmentRepository.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(ApiResponseFactory.Fail("Department not found"));

                if (await _departmentRepository.DepartmentNameExistsAsync(dto.Name, id))
                    return Conflict(ApiResponseFactory.Fail("Department name already exists"));

                _mapper.Map(dto, existing);
                await _departmentRepository.UpdateAsync(existing);

                var updatedDto = _mapper.Map<DepartmentDto>(existing);
                return Ok(ApiResponseFactory.Success(updatedDto, "Department updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating department {id}");
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,HR")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest(ApiResponseFactory.Fail("Invalid department ID"));

            try
            {
                var existing = await _departmentRepository.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(ApiResponseFactory.Fail("Department not found"));

                var employeeCount = await _departmentRepository.GetEmployeeCountByDepartmentAsync(id);
                if (employeeCount > 0)
                    return BadRequest(ApiResponseFactory.Fail("Cannot delete department with active employees"));

                await _departmentRepository.DeleteAsync(id);
                return Ok(ApiResponseFactory.Success($"Department {id} deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting department {id}");
                return StatusCode(500, ApiResponseFactory.Fail("Internal server error"));
            }
        }
    }
}
