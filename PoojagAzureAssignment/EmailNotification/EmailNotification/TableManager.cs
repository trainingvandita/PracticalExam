using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmailNotification.Model;

namespace EmailNotification
{
    public class TableManager
    {
        private CloudTable EmployeeTable;
        public TableManager(string TableName)
        {
            if (string.IsNullOrEmpty(TableName))
            {
                throw new ArgumentNullException("TableName", "Table Name can't be empty");
            }
            try
            {
                EmployeeTable = CreateTable(TableName);
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

        public CloudStorageAccount CreateStorageAccount(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException FormatExceptionObj)
            {
                throw FormatExceptionObj;
            }
            catch (ArgumentException ArgumentExceptionObj)
            {
                throw ArgumentExceptionObj;
            }
            return storageAccount;
        }

        public CloudTable CreateTable(string tableName)
        {
            string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=training2blobstorage;AccountKey=d4swk28SyaP0RkzEssi2Q39Zsg6+rFQ8bv/UvzYfgwHXIqbuYjY1yrUWXXoH50YdTuipEZhnwQ+JS93TMNFRCg==;EndpointSuffix=core.windows.net";
            CloudStorageAccount storageAccount = CreateStorageAccount(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            tableClient.TableClientConfiguration.UseRestExecutorForCosmosEndpoint = true;
            CloudTable table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
            return table;
        }

        //public Employee InsertOrMergeEntity(Employee entity)
        //{
        //    if (entity == null)
        //    {
        //        throw new ArgumentNullException("entity");
        //    }
        //    try
        //    {
        //        TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
        //        TableResult result = EmployeeTable.Execute(insertOrMergeOperation);
        //        Employee insertedCustomer = result.Result as Employee;
        //        return insertedCustomer;
        //    }
        //    catch (StorageException StorageExceptionObj)
        //    {
        //        throw StorageExceptionObj;
        //    }
        //}

        public List<Employee> employeeslist()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                TableQuery<Employee> query = new TableQuery<Employee>();
                employees = EmployeeTable.ExecuteQuery(query).ToList();
                return employees;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return employees;
            }
        }
    }
}
