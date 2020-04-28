using AzureFunctions.Model;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions
{
    public class EmployeeManager
    {
        public static CloudTable CreateTable(string tableName)
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=training2blobstorage;AccountKey=d4swk28SyaP0RkzEssi2Q39Zsg6+rFQ8bv/UvzYfgwHXIqbuYjY1yrUWXXoH50YdTuipEZhnwQ+JS93TMNFRCg==;EndpointSuffix=core.windows.net";           
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(storageConnectionString);            
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());           
            tableClient.TableClientConfiguration.UseRestExecutorForCosmosEndpoint = true;
            CloudTable table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
            return table;
        }

        public static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }
            return storageAccount;
        }

        public static List<Employee> GetEmployees(CloudTable table)
        {
            TableQuery<Employee> query = new TableQuery<Employee>();
            return table.ExecuteQuery(query).ToList();
        }
    }
}
