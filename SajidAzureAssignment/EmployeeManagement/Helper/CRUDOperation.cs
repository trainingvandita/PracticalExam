using EmployeeManagement.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EmployeeManagement.Helper
{
    public class CRUDOperation
    {
        public static async Task<bool> InsertOrMergeEmployeeAsync(CloudTable table, Employee entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                // Create the InsertOrReplace table operation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

                if ((table.ExecuteAsync(insertOrMergeOperation).Result) != null)
                    return true;
                return false;              
            }
            catch (StorageException e)
            {               
                throw;
            }
        }

        public static async Task<List<Employee>> GetEmployeesAsync(CloudTable table)
        {
            TableQuery<Employee> query = new TableQuery<Employee>();
            return table.ExecuteQuery(query).ToList();           
        }
    }
}