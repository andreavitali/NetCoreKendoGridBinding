using System;

namespace NetCoreKendoAngularGridBinding
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int Salary { get; set; }
        public DateTime Recruitment { get; set; }
    }

    public class Department
    {
        public int DepartmentID { get; set; }
        public string Description { get; set; }
    }

    public class EmployeeDTO
    {
        public string FullName { get; set; }
        public string Department { get; set; }
        public int Salary { get; set; }
        public DateTime Recruitment { get; set; }
    }
}
