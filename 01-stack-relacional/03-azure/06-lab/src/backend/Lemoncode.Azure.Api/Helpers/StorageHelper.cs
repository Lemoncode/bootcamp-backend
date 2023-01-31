using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure.Storage;
using Lemoncode.Azure.Models.Configuration;

namespace Lemoncode.Azure.Api.Helpers
{
    public static class StorageHelper
    {

        public static bool IsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg", ".pjpeg", ".svg" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        public static async Task<string> UploadFileToStorage(
            Stream fileStream,
            string fileName,
            StorageOptions storageOptions)
        {
            Uri blobUri = new Uri($"https://{storageOptions.AccountName}.blob.core.windows.net/{storageOptions.ScreenshotsContainer}/{fileName}");
            StorageSharedKeyCredential storageCredentials = new StorageSharedKeyCredential(storageOptions.AccountName, storageOptions.AccountKey);
            BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

            var blobInfo = await blobClient.UploadAsync(fileStream);

            return await Task.FromResult(blobUri.ToString());
        }

        public static async Task<List<string>> GetThumbNailUrls(StorageOptions storageOptions)
        {
            List<string> thumbnailUrls = new List<string>();
            Uri accountUri = new Uri($"https://{storageOptions.AccountName}.blob.core.windows.net/");
            BlobServiceClient blobServiceClient = new BlobServiceClient(accountUri);
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(storageOptions.ScreenshotsContainer);

            if (container.Exists())
            {
                foreach (BlobItem blobItem in container.GetBlobs())
                {
                    thumbnailUrls.Add(container.Uri + "/" + blobItem.Name);
                }
            }

            return await Task.FromResult(thumbnailUrls);
        }

    }
}
