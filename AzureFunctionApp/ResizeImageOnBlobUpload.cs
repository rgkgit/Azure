using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunctionApp
{
    public class ResizeImageOnBlobUpload
    {
        [FunctionName("ResizeImageOnBlobUpload")]
        public void Run([BlobTrigger("resizeimagefunc/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob,
            [Blob("resizeimagefunc-sm/{name}",FileAccess.Write)] Stream myBlobOutput,
            string name, ILogger log)
        {
            try
            {
                log.LogInformation($"Image resize triggered for {name}");

                // Load the original image
                using (var originalImage = Image.FromStream(myBlob))
                {
                    // Calculate the new width and height
                    int newWidth = 800; // Adjust the desired width
                    int newHeight = (int)(((double)newWidth / originalImage.Width) * originalImage.Height);

                    // Create a new Bitmap with the desired size
                    using (var resizedImage = new Bitmap(newWidth, newHeight))
                    {
                        // Resize the image
                        using (var graphics = Graphics.FromImage(resizedImage))
                        {
                            graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                        }

                        // Save the resized image to the output stream
                        resizedImage.Save(resizedImageStream, originalImage.RawFormat);
                    }
                }

                log.LogInformation($"Image resized successfully: {name}");
            }
            catch (Exception ex)
            {
                log.LogError($"Error resizing image: {ex.Message}");
            }
        }
    }
}
