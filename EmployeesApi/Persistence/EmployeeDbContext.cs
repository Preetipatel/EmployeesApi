using EmployeesApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApi.Persistence
{
    public class EmployeeDbContext : DbContext
    {
        public  EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public virtual DbSet<EmployeeEntity> Employees { get; set; }
        public virtual DbSet<AddressEntity> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeEntity>(emp =>
            {
                emp.HasKey("Id");
            });

            modelBuilder.Entity<AddressEntity>(address =>
            {
                address.HasKey("Id");

            });
        }
    }
}
