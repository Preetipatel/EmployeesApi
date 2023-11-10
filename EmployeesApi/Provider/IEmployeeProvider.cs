using EmployeesApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesApi.Provider
{
    public interface IEmployeeProvider
    {
        public Task<IEnumerable<EmployeeEntity>> GetAllEmployeesAsync();

        public Task<EmployeeEntity> AddEmployeeAsync(EmployeeEntity employee);

        //public Task<ActionResult<EmployeeEntity>> AddEmployeeAsync(EmployeeEntity employee);

         public Task<EmployeeEntity> UpdateEmployeeAsync(EmployeeEntity employee);

        //public Task<ActionResult<EmployeeEntity>> UpdateEmployeeAsync(EmployeeEntity employee);
        public Task DeleteEmployeeById(int id);

        public bool IsEmployeeExists(EmployeeEntity employee);
    }
}
