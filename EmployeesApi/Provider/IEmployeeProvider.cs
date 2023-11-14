using Employees.Contracts;

namespace EmployeesApi.Provider
{
    public interface IEmployeeProvider
    {
        public Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync();

        public Task<EmployeeResponse> AddEmployeeAsync(AddEmployeeRequest employeeRequest);

        public Task<EmployeeResponse> UpdateEmployeeAsync(EmployeeResponse employee);

        public Task DeleteEmployeeById(Guid id);

        public Task<IEnumerable<EmployeeResponse>> SearchAsync(string searchText);
    }
}
