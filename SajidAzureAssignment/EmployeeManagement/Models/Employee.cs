using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManagement.Models
{
    public class Employee : TableEntity
    {
        public Employee()
        { }
        public Employee(string FirstName, string LastName)
        {
            PartitionKey = FirstName;
            RowKey = LastName;
        }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Resume { get; set; }
    }
}