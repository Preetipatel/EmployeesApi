using EmployeesApi.Domain;
using EmployeesApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApi.Provider
{
    public class EmployeeProvider : IEmployeeProvider
    {
        private readonly EmployeeDbContext _employeeDbContext;
        private const int MAX_AGE = 65;
        private const int MIN_AGE = 18;
        public EmployeeProvider(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        public async Task<IEnumerable<EmployeeEntity>> GetAllEmployeesAsync()
        {
            var employees = await _employeeDbContext.Employees.AsNoTracking().ToListAsync();
            return employees;
        }

        //Ignores the id provded in payload and uses the db generated id
        public async Task<EmployeeEntity> AddEmployeeAsync(EmployeeEntity employee)
        {
            BasicValidation(employee);

            if (IsEmployeeExists(employee))
            {
                throw new InvalidOperationException("An employee with same firstname, lastname and email already exists. ");
            }
            await _employeeDbContext.Employees.AddAsync(employee);
            await _employeeDbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<EmployeeEntity> UpdateEmployeeAsync(EmployeeEntity employee)
        {
            BasicValidation(employee);

            var existingEmployee = await _employeeDbContext.Employees.FirstOrDefaultAsync(emp => emp.Id == employee.Id);
            if (existingEmployee == null)
            {
                throw new InvalidOperationException($"Employee with Id {employee.Id} not found");
            }
            if (!string.Equals(existingEmployee.FirstName, employee.FirstName)
                || !string.Equals(existingEmployee.LastName, employee.LastName)
                || !string.Equals(existingEmployee.Email, employee.Email))
            {
                if (IsEmployeeExists(employee))
                {
                    throw new InvalidOperationException("An employee with same firstname, lastname and email already exists. ");
                }
            }
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Age = employee.Age;

            await _employeeDbContext.SaveChangesAsync();
            return existingEmployee;
        }

        public async Task DeleteEmployeeById(int id)
        {
            var existingEntity = await _employeeDbContext.Employees.FirstOrDefaultAsync(emp => emp.Id == id);
            if (existingEntity == null)
            {
                throw new InvalidOperationException($"Employee with {id} not found");
            }
            _employeeDbContext.Remove(existingEntity);
            await _employeeDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmployeeEntity>> SearchAsync(string searchText)
        {
            var query = _employeeDbContext.Employees.AsQueryable();

            if (string.IsNullOrEmpty(searchText))
            {
                return await query.AsNoTracking().ToListAsync();
            }
            var result = await query.Where(e => string.Equals(e.FirstName, searchText, StringComparison.InvariantCultureIgnoreCase)
                                       || string.Equals(e.LastName, searchText, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();

            return result;
        }

        private static void BasicValidation(EmployeeEntity employee)
        {
            if (employee == null)
            {
                throw new InvalidOperationException($"Employee with Id {employee.Id} not found");
            }
            if(string.IsNullOrWhiteSpace(employee.FirstName))
            {
                throw new InvalidOperationException($"FirstName can not be empty");
            }
            if (string.IsNullOrWhiteSpace(employee.LastName))
            {
                throw new InvalidOperationException($"LastName can not be empty");
            }
            if (string.IsNullOrWhiteSpace(employee.Email))
            {
                throw new InvalidOperationException($"Email can not be empty");
            }
            if (employee.Age > MAX_AGE || employee.Age < MIN_AGE)
            {
                throw new InvalidOperationException($"Employee's age should lie between {MIN_AGE} to {MAX_AGE}.");
            }
        }    

        public bool IsEmployeeExists(EmployeeEntity employee)
        {
            return _employeeDbContext.Employees
                                        .Any(emp =>
                                             string.Equals(emp.FirstName, employee.FirstName) &&
                                             string.Equals(emp.LastName, employee.LastName) &&
                                             string.Equals(emp.Email, employee.Email));
        }
    }
}
