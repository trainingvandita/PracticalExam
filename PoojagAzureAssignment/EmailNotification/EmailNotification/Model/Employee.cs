using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailNotification.Model
{
    public class Employee : TableEntity
    {
        public Employee()
        {
        }

        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Resume { get; set; }
    }

    
    
}
