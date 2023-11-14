﻿using Employees.Contracts;
using EmployeesApi.Domain;
using EmployeesApi.Provider;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesApi.Area.V1.Controllers
{
    [ApiController]
    [Area("1.0")]
    [Route("[area]/[controller]")]

    public class EmployeesController : Controller
    {
        private readonly IEmployeeProvider _employeeProvider;
        public EmployeesController(IEmployeeProvider employeeProvider)
        {
            _employeeProvider = employeeProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeResponse>>> GetEmployeesAsync()
        {
            var employees = await _employeeProvider.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> AddEmployeeAsync([FromBody] AddEmployeeRequest employee)
        {
            EmployeeResponse newEmployee;
            try
            {
               newEmployee = await _employeeProvider.AddEmployeeAsync(employee);
                return StatusCode(201, newEmployee);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, new { status = "error", message = "An internal server error occurred." });
            }
        }

        [HttpPut]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployeeAsync([FromBody] EmployeeResponse employee)
        {
            try
            {
                await _employeeProvider.UpdateEmployeeAsync(employee);
                return Ok(employee);

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, new { status = "error", message = "An internal server error occurred." });
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteEmployeeById(Guid id)
        {
            try
            {
                await _employeeProvider.DeleteEmployeeById(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch
            {
                return StatusCode(500, new { status = "error", message = "An internal server error occurred." });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EmployeeResponse>>> SearchEmployeeByName([FromQuery] string searchText)
        {
            var result = await _employeeProvider.SearchAsync(searchText);
            return Ok(result);
        }
    }
}
