
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Linq;
using System.Collections.Generic;

using BlobWebApplication.Models;

namespace BlobWebApplication
{
    public class TableManager
    {
        private CloudTable EmployeeTable;
        public TableManager(string TableName)
        {
            if (string.IsNullOrEmpty(TableName))
            {
                throw new ArgumentNullException(Constants.Table, Constants.TableNameError);
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

        public CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
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
            Keymanager keyVaultManager = new Keymanager();
            string ConnectionString = keyVaultManager.GetValueFromAzureVault(Constants.SecretName);
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            tableClient.TableClientConfiguration.UseRestExecutorForCosmosEndpoint = true;
            CloudTable table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
            return table;
        }

        public ChiragInfo InsertOrMergeEntity(ChiragInfo entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
                TableResult result = EmployeeTable.Execute(insertOrMergeOperation);
                ChiragInfo insertedCustomer = result.Result as ChiragInfo;
                return insertedCustomer;
            }
            catch (StorageException StorageExceptionObj)
            {
                throw StorageExceptionObj;
            }
        }

        public List<ChiragInfo> employees()
        {
            List<ChiragInfo> employees = new List<ChiragInfo>();
            try
            {
                TableQuery<ChiragInfo> query = new TableQuery<ChiragInfo>();
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