namespace AzureBlobProject.Models
{
    public class ContainerModel
    {
        public string Name { get; set; }
        public List<string> Blobs { get; set; }
    }
    public class AccountModel
    {
        public string AccountName { get; set; }
        public List<ContainerModel> Containers { get; set; }
    }
}
