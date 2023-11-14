using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesApi.Domain
{
    public class EmployeeEntity
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public AddressEntity? Address { get; set; }
        
    }
}
