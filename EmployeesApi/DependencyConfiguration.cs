using EmployeesApi.Domain;
using EmployeesApi.Persistence;
using EmployeesApi.Provider;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EmployeesApi
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddDbContext<EmployeeDbContext>(options =>
            {
                options.UseInMemoryDatabase("EmployeeDatabase");
            });
            var serviceProvider = services.BuildServiceProvider();
            SeedEmployeeData(serviceProvider);
            services.AddScoped<IEmployeeProvider, EmployeeProvider>();

            services.AddMappings();
            return services;
        }

        private static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }

        private static void SeedEmployeeData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var employeeDbContext = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();

                List<EmployeeEntity> employees = new List<EmployeeEntity>()
                {
                    new EmployeeEntity()
                    {
                        Id  = Guid.NewGuid(),
                        FirstName = "John",
                        LastName = "Cruise",
                        Email = "John.cruise@gmail.com",
                        Age = 35,
                        Address = new AddressEntity()
                        {
                            City = "Raipur",
                            StreetName = "Mowa",
                            Country = "India",
                            PostalCode = "123"

                        }
                    },
                    new EmployeeEntity()
                    {
                        Id  = Guid.NewGuid(),
                        FirstName = "Nikki",
                        LastName = "Trump",
                        Email = "Nikki.trump@gmail.com",
                        Age = 33,
                        Address = new AddressEntity()
                        {
                            City = "Bilaspur",
                            StreetName = "Veerendra Nagar",
                            Country = "India",
                            PostalCode = "123"

                        }
                    },
                    new EmployeeEntity()
                    {
                        Id  = Guid.NewGuid(),
                        FirstName = "Harry",
                        LastName = "Junior",
                        Email = "Harry.junior@gmail.com",
                        Age = 38,
                        Address = new AddressEntity()
                        {
                            City = "Raigarh",
                            StreetName = "Rajiv Nagar",
                            Country = "India",
                            PostalCode = "123"

                        }
                    }
                };
                employeeDbContext.AddRange(employees);
                employeeDbContext.SaveChanges();
            }
        }
    }
}
