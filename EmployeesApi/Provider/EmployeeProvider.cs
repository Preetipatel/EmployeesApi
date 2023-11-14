using Employees.Contracts;
using EmployeesApi.Domain;
using EmployeesApi.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApi.Provider
{
    public class EmployeeProvider : IEmployeeProvider
    {
        private readonly EmployeeDbContext _employeeDbContext;
        private readonly IMapper _mapper;
        private const int MAX_AGE = 65;
        private const int MIN_AGE = 18;

        public EmployeeProvider(
            EmployeeDbContext employeeDbContext,
            IMapper mapper)
        {
            _employeeDbContext = employeeDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync()
        {
            var employees = await _employeeDbContext.Employees.Include(x => x.Address).AsNoTracking().ToListAsync();
            var employeeResponse = _mapper.Map<IEnumerable<EmployeeResponse>>(employees);
            return employeeResponse;
        }

         public async Task<EmployeeResponse> AddEmployeeAsync(AddEmployeeRequest employeeRequest)
         {
            var employee = _mapper.Map<EmployeeEntity>(employeeRequest);
            employee.Id = Guid.NewGuid();
            BasicValidation(employee);

            if (IsEmployeeExists(employee))
            {
                throw new InvalidOperationException("An employee with same firstname, lastname and email already exists. ");
            }
            await _employeeDbContext.Employees.AddAsync(employee);
            await _employeeDbContext.SaveChangesAsync();

            var response = _mapper.Map<EmployeeResponse>(employee);
            return response;
         }

        /*
         * Updates employee fields
         * Checks the duplicate value for firstname, lastname email before performing update
         */
        public async Task<EmployeeResponse> UpdateEmployeeAsync(EmployeeResponse employee)
        {
            
            var existingEmployee = await _employeeDbContext.Employees
                                        .Include(e => e.Address)
                                        .FirstOrDefaultAsync(emp => emp.Id == employee.Id);

            if (existingEmployee == null)
            {
                throw new InvalidOperationException($"Employee with Id {employee.Id} not found");
            }

            var updatedEmployee = _mapper.Map<EmployeeEntity>(employee);
            BasicValidation(updatedEmployee);

            if (!string.Equals(existingEmployee.FirstName, employee.FirstName)
                || !string.Equals(existingEmployee.LastName, employee.LastName)
                || !string.Equals(existingEmployee.Email, employee.Email))
            {
                if (IsEmployeeExists(updatedEmployee))
                {
                    throw new InvalidOperationException("An employee with same firstname, lastname and email already exists. ");
                }
            }

            existingEmployee.FirstName = updatedEmployee.FirstName;
            existingEmployee.LastName = updatedEmployee.LastName;
            existingEmployee.Email = updatedEmployee.Email;
            existingEmployee.Age = updatedEmployee.Age;
            existingEmployee.Address = updatedEmployee.Address;

            await _employeeDbContext.SaveChangesAsync();
            var response = _mapper.Map<EmployeeResponse>(existingEmployee);
            return response;
        }

        public async Task DeleteEmployeeById(Guid id)
        {
            var existingEntity = await _employeeDbContext.Employees
                                       .Include(e =>e.Address)
                                       .FirstOrDefaultAsync(emp => emp.Id == id);

            if (existingEntity == null)
            {
                throw new InvalidOperationException($"Employee with {id} not found");
            }

            _employeeDbContext.Remove(existingEntity);
            await _employeeDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmployeeResponse>> SearchAsync(string searchText)
        {
            var query = _employeeDbContext.Employees.AsQueryable();
            
            searchText = searchText.ToLower();
            var employees = await query.Where(e => e.FirstName.ToLower().Contains(searchText)
                                            || e.LastName.ToLower().Contains(searchText))
                                   .ToListAsync();

            return _mapper.Map<IEnumerable<EmployeeResponse>>(employees);
        }

        private static void BasicValidation(EmployeeEntity employee)
        {
            if (employee == null)
            {
                throw new InvalidOperationException($"Employee can not be null");
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

        private bool IsEmployeeExists(EmployeeEntity employee)
        {
            return _employeeDbContext.Employees
                                        .Any(emp =>
                                             string.Equals(emp.FirstName, employee.FirstName) &&
                                             string.Equals(emp.LastName, employee.LastName) &&
                                             string.Equals(emp.Email, employee.Email));
        }
    }
}
