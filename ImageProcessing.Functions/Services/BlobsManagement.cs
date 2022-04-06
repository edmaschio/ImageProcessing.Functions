using Azure.Storage.Blobs;
using ImageProcessing.Functions.Services.Interfaces;

namespace ImageProcessing.Functions.Services
{
    public class BlobsManagement : IBlobsManagement
    {
        public async Task<string> UploadFile(string containerName, string fileName, byte[] file, string connectionString)
        {
            try
            {
                // Get reference to a container and then create it
                var container = new BlobContainerClient(connectionString, containerName);
                await container.CreateIfNotExistsAsync();
                await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

                // Get a reference to a blob in a container
                var blob = container.GetBlobClient(fileName);

                Stream str = new MemoryStream(file);
                // Upload local file
                await blob.UploadAsync(str);

                return blob.Uri.AbsoluteUri;
            }
            catch (Exception ex)
            {
                // TODO handling error
                Console.WriteLine(ex);
                return string.Empty;
            }
        }
    }
}
