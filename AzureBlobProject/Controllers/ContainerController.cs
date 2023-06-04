using AzureBlobProject.Models;
using AzureBlobProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly IContainerService _containerService;
        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;
        }

        [HttpGet("Containers")]
        public async Task<ActionResult> GetAllContainers()
        {
            return Ok(await _containerService.GetAllContainersAsync());
        }
        [HttpGet("All")]
        public async Task<ActionResult> GetAllContainersAndBlobs()
        {
            return Ok(await _containerService.GetContainerAndBlobsAsync());
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create(ContainerModel containerModel)
        {
            await _containerService.CreateContainerAsync(containerModel.Name);
            return Ok("Created Successfully");
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(ContainerModel containerModel)
        {
            await _containerService.DeleteContainerAsync(containerModel.Name);
            return Ok("Deleted Successfully");
        }
    }
}
