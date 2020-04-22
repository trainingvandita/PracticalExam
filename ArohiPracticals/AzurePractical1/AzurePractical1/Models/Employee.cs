using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzurePractical1.Models
{
    public class Employee : TableEntity
    {
        public Employee() { }

        public Employee(string EmpFName, string EmpLName)
        {
            PartitionKey = EmpLName;
            RowKey = EmpFName;
        }

       
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string FileUpload { get; set; } 
    }
}