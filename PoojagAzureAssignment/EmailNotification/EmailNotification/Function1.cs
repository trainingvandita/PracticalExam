using System;
using System.Collections.Generic;
using EmailNotification.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;


namespace EmailNotification
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("00:00:03")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            Execute();
        }
        private static void Execute()
        {
            TableManager tableManager = new TableManager("Poojag");
            List<Employee> employees = tableManager.employeeslist();
            Console.WriteLine("Registered Employees");
            foreach (Employee employee in employees)
            {
                Console.WriteLine(employee.FirstName + " " + employee.LastName);
            }
           
    
            Console.WriteLine("Email Sent To:HR@abc.com  at: " + DateTime.Now);
        }
    }
}
