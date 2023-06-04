using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using AzureBlobProject.Models;

namespace AzureBlobProject.Services
{
    public interface IBlobService
    {
        Task<List<string>> GetAllBlobsAsync(string containerName);
        Task<string> GetBlobAsync(string name, string containerName);
        Task<bool> UploadBlobAsync(string name, string containerName, BlobModel blobModel);
        Task<bool> DeleteBlobAsync(string name,string containerName);
    }
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        public async Task<bool> DeleteBlobAsync(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(name);
            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllBlobsAsync(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();
            var blobItems = new List<string>();
            await foreach(var item in blobs)
            {
                blobItems.Add(item.Name);
            }
            return blobItems;
        }

        public async Task<string> GetBlobAsync(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(name);
            string blobUrl = blobClient.Uri.AbsoluteUri;


            //blobUrl = await GetBlobUrlWithSasToken(blobClient) //Generate SAS token in Blob level
            //blobUrl = await GetBlobUrlWithContainerSasToken(blobContainerClient, blobClient); //Generate SAS token in container level

            return blobUrl;
        }

        public async Task<string> GetBlobUrlWithSasToken(BlobClient blobClient)
        {
            string blobUrl = blobClient.Uri.AbsoluteUri;
            if(blobClient.CanGenerateSasUri)
            {
                BlobSasBuilder sasBuilder = new()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b", //b -> blob
                    ExpiresOn = DateTime.UtcNow.AddHours(1),
                };
                sasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

                //For Multiple permissions
                //sasBuilder.SetPermissions(BlobAccountSasPermissions.Read | BlobAccountSasPermissions.Write);

                return blobClient.GenerateSasUri(sasBuilder).AbsoluteUri;
            }
            return blobUrl;
        }

        public async Task<string> GetBlobUrlWithContainerSasToken(BlobContainerClient blobContainerClient, BlobClient blobClient)
        {
            string blobUrl = blobClient.Uri.AbsoluteUri;
            
            if (blobContainerClient.CanGenerateSasUri)
            {
                BlobSasBuilder sasBuilder = new()
                {
                    BlobContainerName = blobContainerClient.Name,
                    Resource = "c", //b -> container
                    ExpiresOn = DateTime.UtcNow.AddHours(1),
                };
                sasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

                //For Multiple permissions
                //sasBuilder.SetPermissions(BlobAccountSasPermissions.Read | BlobAccountSasPermissions.Write);

                string sasContainerSinature = blobContainerClient.GenerateSasUri(sasBuilder).AbsoluteUri.Split('?')[1].ToString();

                blobUrl += "?" + sasContainerSinature;
            }
            return blobUrl;
        }

        public async Task<bool> UploadBlobAsync(string name, string containerName, BlobModel blobModel)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(name);
            var httpHeaders = new BlobHttpHeaders
            {
                ContentType = blobModel.File.ContentType
            };
            IDictionary<string, string> metaData = new Dictionary<string, string>
            {
                { "title", blobModel.Title }
            };
            metaData["comment"] = blobModel.Comment;
            var result = await blobClient.UploadAsync(blobModel.File.OpenReadStream(), httpHeaders, metaData);

            //Remove all meta data for a blob
            //IDictionary<string,string> removeMetaData = new Dictionary<string, string>();
            //await blobClient.SetMetadataAsync(removeMetaData);

            //Remove single meta data for a blob
            //metaData.Remove("title");
            //await blobClient.SetMetadataAsync(metaData);

            return result != null;
        }
    }
}
