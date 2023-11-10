using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesApi.Domain
{
    public class EmployeeEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }
        
    }
}
