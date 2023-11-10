using EmployeesApi.Domain;
using EmployeesApi.Persistence;
using EmployeesApi.Provider;
using Microsoft.EntityFrameworkCore;

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
                    Id  = 1,
                    FirstName = "John",
                    LastName = "Cruise",
                    Email = "John.cruise@gmail.com",
                    Age = 35
                    },
                new EmployeeEntity()
                {
                    Id  = 2,
                    FirstName = "Nikki",
                    LastName = "Trump",
                    Email = "Nikki.trump@gmail.com",
                    Age = 33
                },
                new EmployeeEntity()
                {
                    Id  = 3,
                    FirstName = "Harry",
                    LastName = "Junior",
                    Email = "Harry.junior@gmail.com",
                    Age = 38
                }
            };
                employeeDbContext.AddRange(employees);
                employeeDbContext.SaveChanges();
            }
        }
    }
}
