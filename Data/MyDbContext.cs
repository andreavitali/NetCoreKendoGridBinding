using Microsoft.EntityFrameworkCore;
using System;

namespace NetCoreKendoAngularGridBinding
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department() { DepartmentID = 1, Description = "R&D" },
                new Department() { DepartmentID = 2, Description = "Accounting" });

            modelBuilder.Entity<Employee>().HasData(
                 new Employee { EmployeeID = 1, FirstName = "Bill", LastName = "Jones", DepartmentId = 1, Salary = 2000, Recruitment = new DateTime(2015, 1, 1) },
                 new Employee { EmployeeID = 2, FirstName = "Rob", LastName = "Johnson", DepartmentId = 1, Salary = 1800, Recruitment = new DateTime(2016, 6, 1) },
                 new Employee { EmployeeID = 3, FirstName = "Jane", LastName = "Smith", DepartmentId = 2, Salary = 1700, Recruitment = new DateTime(2016, 10, 20) });
        }
    }
}
