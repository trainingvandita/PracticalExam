using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using BlobWebApplication.Models;
using BlobWebApplication;
using System.Collections.Generic;
using System.Linq;

namespace TImerAzure
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            TableManager BManagerObj = new TableManager("ChiragInfo");
            List<ChiragInfo> SutdentListObj = BManagerObj.employees();
            ChiragInfo StudentObj = SutdentListObj.FirstOrDefault();
            foreach (var singleData in SutdentListObj)
                log.LogInformation(singleData.RowKey + " " + singleData.PartitionKey + " " + $"{ DateTime.Now}");
            string EmailID = "HR@abc.com";
            Console.WriteLine("Email Sent To:" + EmailID + "at:" + DateTime.Now);
            //0 0 0 * * *   
        }
    }
}
