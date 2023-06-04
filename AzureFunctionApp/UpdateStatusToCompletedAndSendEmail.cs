using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AzureFunctionApp.Data;
using AzureFunctionApp.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionApp
{
    public class UpdateStatusToCompletedAndSendEmail
    {
        private readonly AzureDbContext _context;
        public UpdateStatusToCompletedAndSendEmail(AzureDbContext context)
        {
            _context = context;
        }
        [FunctionName("UpdateStatusToCompletedAndSendEmail")]
        public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            List<SalesRequestModel> salesRequests = _context.SalesRequests.Where(x => x.Status == "Image Processed").ToList();
            foreach(SalesRequestModel model in salesRequests)
            {
                model.Status = "Completed";
            }
            _context.UpdateRange(salesRequests);
            _context.SaveChanges();
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
