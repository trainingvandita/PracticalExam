using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using System.Web;

namespace EmployeeManagement.Helper
{
    public class CreateTable
    {
        public static async Task<CloudTable> CreateTableAsync(string tableName)
        {
            KeyVaultManager keyVaultManager = new KeyVaultManager();
            string storageConnectionString = keyVaultManager.GetValueFromAzureVault();
            //string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=training2blobstorage;AccountKey=d4swk28SyaP0RkzEssi2Q39Zsg6+rFQ8bv/UvzYfgwHXIqbuYjY1yrUWXXoH50YdTuipEZhnwQ+JS93TMNFRCg==;EndpointSuffix=core.windows.net";
            //string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=sajidalidatabase;AccountKey=Gb3rZHuJPqoWIc53GB12tyBoecSE0hmVNMvsTIt57pEA8eebyXOlXYN25FesSQYwaHQLJmtkrzSriFJXVK1QHg==;TableEndpoint=https://sajidalidatabase.table.cosmos.azure.com:443/;";

            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(storageConnectionString);

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            Console.WriteLine("Create a Table for the demo");
            tableClient.TableClientConfiguration.UseRestExecutorForCosmosEndpoint = true;
            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExistsAsync();

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
    }
}