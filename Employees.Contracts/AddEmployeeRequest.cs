namespace Employees.Contracts
{
    public class AddEmployeeRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public Address Address { get; set; }
    }
    public class Address
    {
        public string City { get; set; }
        public string StreetName { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}