using System;
using System.Collections.Generic;
using AzureAssignment;
using AzureAssignment.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;


namespace EmailNotificationAzureFunction
{
    public static class AzureEmailFunction
    {
        [FunctionName("AzureEmailFunction")]
        public static void Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log)
        {

            TableManager TableManagerObj = new TableManager("DhartiAssignmentEmployeeTable");
            List<Employee> EmployeeListObj = TableManagerObj.RetrieveEntity<Employee>();
          // Employee Emp = EmployeeListObj.FirstOrDefault();
            foreach (var singleData in EmployeeListObj)
                log.LogInformation(singleData.FirstName + " " + singleData.LastName + " " + $"{ DateTime.Now}");
            string EmailID = "HR@abc.com";
            Console.WriteLine("Email Sent To:" + EmailID + "at:" + DateTime.Now);
            //0 0 0 * * *   
        }
    }
}
