using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobProject.Models;

namespace AzureBlobProject.Services
{
    public interface IContainerService
    {
        Task<List<string>> GetAllContainersAsync();
        Task<AccountModel> GetContainerAndBlobsAsync();
        Task CreateContainerAsync(string containerName);
        Task DeleteContainerAsync(string containerName);
    }
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IBlobService _blobService;

        public ContainerService(BlobServiceClient blobServiceClient, IBlobService blobService)
        {
            _blobServiceClient = blobServiceClient;
            _blobService = blobService;
        }
        public async Task CreateContainerAsync(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }

        public async Task DeleteContainerAsync(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainersAsync()
        {
            List<string> containers = new();
            await foreach(BlobContainerItem blobContainerItem in _blobServiceClient.GetBlobContainersAsync())
            {
                containers.Add(blobContainerItem.Name);
            }

            return containers;
        }

        public async Task<AccountModel> GetContainerAndBlobsAsync()
        {
            AccountModel accountModel = new();
            accountModel.AccountName = _blobServiceClient.AccountName;

            accountModel.Containers = new();
            await foreach (BlobContainerItem blobContainerItem in _blobServiceClient.GetBlobContainersAsync())
            {
                ContainerModel container =  new() { Name = blobContainerItem.Name };
                container.Blobs = new();
                BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobContainerItem.Name);

                await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
                {
                    BlobClient blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                    BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
                    string blob = blobItem.Name;
                    if(blobProperties.Metadata.ContainsKey("title"))
                    {
                        blob += " (" + blobProperties.Metadata["title"] + ")";
                    }

                    container.Blobs.Add(blob);
                }
                accountModel.Containers.Add(container);
            }
            return accountModel;
        }
    }
}
