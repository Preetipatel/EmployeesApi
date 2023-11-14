namespace EmployeesApi.Domain
{
    public class AddressEntity
    {
        public int Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string City { get; set; }
        public string StreetName { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public EmployeeEntity Employee { get; set; }
    }
}
