using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureAssignment.Models
{
    public class Employee : TableEntity
    {
        public Employee() { }
        public Employee(string Lastname, string Firstname)
        {
            PartitionKey = Lastname;
            RowKey = Firstname;
        }
       
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Url { get; set; }

    }
}