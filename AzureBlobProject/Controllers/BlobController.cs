using AzureBlobProject.Models;
using AzureBlobProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly IBlobService _blobService;
        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpGet("All")]
        public async Task<ActionResult> GetAllBlobs(string containerName)
        {
            return Ok(await _blobService.GetAllBlobsAsync(containerName));
        }
        [HttpGet("Single")]
        public async Task<ActionResult> GetBlob(string name,string containerName)
        {
            return Ok(await _blobService.GetBlobAsync(name, containerName));
        }
        [HttpPost("Upload")]
        public async Task<ActionResult> Upload(string containerName, IFormFile file, string title, string comment)
        {
            if(file?.Length < 1) { return BadRequest("Invalid Input"); }
            BlobModel blobModel = new BlobModel() { Title = title, Comment = comment, File = file };
            var fileName  = Path.GetFileNameWithoutExtension(blobModel.File.FileName)+"_"+Guid.NewGuid()+Path.GetExtension(blobModel.File.FileName);
            var result = await _blobService.UploadBlobAsync(fileName, containerName, blobModel);
            if(result)
                return Ok("Uploaded Successfully");
            return StatusCode(500, "Error Occured while upload");
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(string name, string containerName)
        {
            await _blobService.DeleteBlobAsync(name,containerName);
            return Ok("Deleted Successfully");
        }
    }
}
