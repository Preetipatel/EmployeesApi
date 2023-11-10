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
        public async Task<ActionResult<IEnumerable<EmployeeEntity>>> GetEmployeesAsync()
        {
            var employees = await _employeeProvider.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeEntity>> AddEmployeeAsync([FromBody] EmployeeEntity employee)
        {
           /* if (employee == null)
            {
                return BadRequest(new { status = "error", message = "Invalid input. The request body cannot be empty." });
            }
            if(_employeeProvider.IsEmployeeExists(employee))
            {
                return BadRequest(new { status = "error", message = "An employee with same firstname, lastname, emial already exists." });
            }*/
            EmployeeEntity newEmployee;
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
        public async Task<ActionResult<EmployeeEntity>> UpdateEmployeeAsync([FromBody] EmployeeEntity employee)
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
        public async Task<ActionResult> DeleteEmployeeById(int id)
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
    }
}
