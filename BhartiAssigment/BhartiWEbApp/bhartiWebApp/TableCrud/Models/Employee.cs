using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TableCrud.Models
{
    public class Employee:TableEntity
    {
        public Employee() { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public String Mobile { get; set; }
        public string Address { get; set; }
        public string Path { get; set; }
        public bool? IsActive { get; set; }
    }
}