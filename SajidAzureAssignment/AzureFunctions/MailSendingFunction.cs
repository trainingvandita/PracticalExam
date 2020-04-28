using System;
using System.Collections.Generic;
using System.Linq;
using AzureFunctions.Model;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctions
{
    public static class MailSendingFunction
    {
        [FunctionName("MailSendingFunction")]
        public static void Run([TimerTrigger("0 30 9 * * *")]TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogInformation("Timer is running late!");
            }
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            Execute();
        }

        public static void Execute()
        {
            CloudTable table = EmployeeManager.CreateTable("SajidAliEmployee");           
            List<Employee> employees = EmployeeManager.GetEmployees(table);

            Console.WriteLine("----Registered Employees are----");
            foreach (Employee employee in employees)
            {
                Console.WriteLine(employee.PartitionKey + " " + employee.RowKey);
            }
            string EmailID = "HR@abc.com";
            //Write Logic To Send Mail
            Console.WriteLine("Email Sent To: " + EmailID + " at: " + DateTime.Now);
        }
    }
}
