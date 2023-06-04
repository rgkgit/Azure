using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctionApp.Models;

namespace AzureFunctionApp
{
    public static class OnSalesOrderWriteToQueue
    {
        [FunctionName("OnSalesOrderWriteToQueue")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue("SalesOrderInBound",Connection ="AzureWebJobsStorage")] IAsyncCollector<SalesRequestModel> salesRequestQueue,
            ILogger log)
        {
            log.LogInformation("Sales Order Request received by OnSalesOrderWriteToQueue function");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            SalesRequestModel data = JsonConvert.DeserializeObject<SalesRequestModel>(requestBody);
            
            await salesRequestQueue.AddAsync(data);

            string responseMessage = "Sales Order Request has been received for " + data.Name;

            return new OkObjectResult(responseMessage);
        }
    }
}
