using System.Text;
using AzureFunctionProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AzureFunctionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        static readonly HttpClient httpClient = new HttpClient();
        public SalesOrderController()
        {
            
        }

        [HttpPost("Request")]
        public async Task<IActionResult> OrderRequest(SalesRequestModel salesRequestModel,IFormFile formFile)
        {
            salesRequestModel.Id = Guid.NewGuid().ToString();
            using(var content = new StringContent(JsonConvert.SerializeObject(salesRequestModel),Encoding.UTF8,"application/json")) 
            {
                HttpResponseMessage response = await httpClient.PostAsync("http://localhost:7041/api/OnSalesOrderWriteToQueue", content);
                string returnValue = response.Content.ReadAsStringAsync().Result ;
                return Ok(returnValue);
            }
        }
    }
}
