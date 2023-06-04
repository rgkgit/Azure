﻿namespace AzureBlobProject.Models
{
    public class BlobModel
    {
        public string Title { get; set; }
        public string Comment { get; set; }
        public string Url { get; set; }
        public IFormFile File { get; set; }
    }
}
