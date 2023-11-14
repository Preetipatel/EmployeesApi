using Employees.Contracts;
using EmployeesApi.Domain;
using Mapster;

namespace EmployeesApi.Mapper
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AddEmployeeRequest, EmployeeEntity>()
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Age, src => src.Age)
                .Map(dest => dest.Address.City, src => src.Address.City)
                .Map(dest => dest.Address.Country, src => src.Address.Country)
                .Map(dest => dest.Address.PostalCode, src => src.Address.PostalCode)
                .Map(dest => dest.Address.StreetName, src => src.Address.StreetName)
                .IgnoreNonMapped(true);

            config.NewConfig<EmployeeEntity, EmployeeResponse>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Age, src => src.Age)
                .Map(dest => dest.Address.City, src => src.Address.City)
                .Map(dest => dest.Address.Country, src => src.Address.Country)
                .Map(dest => dest.Address.PostalCode, src => src.Address.PostalCode)
                .Map(dest => dest.Address.StreetName, src => src.Address.StreetName)
                .IgnoreNonMapped(true);
        }
    }
}
