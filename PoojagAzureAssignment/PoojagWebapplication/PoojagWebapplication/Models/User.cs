
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoojagWebapplication.Models
{
    public class User : TableEntity
    {
        public User() { }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string Address { get; set; }

    public string Resume { get; set; }
     }
}
