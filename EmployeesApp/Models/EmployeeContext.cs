using Microsoft.EntityFrameworkCore;

namespace EmployeesApp.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasData
                (
                    new Employee 
                    {
                        Id = Guid.NewGuid(),
                        Name = "Adam",
                        AccountNumber = "123-1234567890-12",
                        Age = 30
                    },
                    new Employee 
                    { 
                        Id = Guid.NewGuid(), 
                        Name = "Eve", 
                        AccountNumber = "999-9999999999-99", 
                        Age = 28
                    }
                 );
        }

        public DbSet<Employee>? Employees { get; set; }
    }
}
