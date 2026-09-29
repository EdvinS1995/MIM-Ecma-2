using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentManagerRepo.Entity
{
  
    public class EmployeeList
    {
        public Executiontime ExecutionTime { get; set; }
        public Customer Customer { get; set; }
        public Counts Counts { get; set; }
        public Sort Sort { get; set; }
        public List<SimpleItem> Items { get; set; }
    }

    public class Executiontime
    {
        public string Minutes { get; set; }
        public string Seconds { get; set; }
        public string Milliseconds { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string ThirdPartyCustomerId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string ApiKey { get; set; }
        public Rootdepartment RootDepartment { get; set; }
    }

    public class Rootdepartment
    {
        public int Id { get; set; }
        public string ThirdPartyId { get; set; }
        public string Name { get; set; }
        public object Details { get; set; }
    }

    public class Counts
    {
        public int SearchCount { get; set; }
        public int SkipCount { get; set; }
        public int TakeCount { get; set; }
        public int TotalCount { get; set; }
    }

    public class Sort
    {
        public string Field { get; set; }
        public int Direction { get; set; }
    }

    public class SimpleItem
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public int Gender { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime EmploymentDatetime { get; set; }
        public DateTime LastUpdated { get; set; }
        public object Details { get; set; }
    }

}
