using System;
using AzureFunctionApp.Data;
using AzureFunctionApp.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionApp
{
    public class OnQueueTriggerUpdateDatabase
    {
        private readonly AzureDbContext _context;
        public OnQueueTriggerUpdateDatabase(AzureDbContext context)
        {
            _context = context;
        }
        [FunctionName("OnQueueTriggerUpdateDatabase")]
        public void Run([QueueTrigger("SalesOrderInBound", Connection = "AzureWebJobsStorage")]SalesRequestModel myQueueItem, ILogger log)
        {
            myQueueItem.Status = "Submitted";
            _context.SalesRequests.Add(myQueueItem);
            _context.SaveChanges();
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
