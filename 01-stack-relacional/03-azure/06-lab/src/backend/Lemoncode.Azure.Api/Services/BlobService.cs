using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Lemoncode.Azure.Models.Configuration;
using Microsoft.Extensions.Options;

namespace Lemoncode.Azure.Api.Services
{
    public class BlobService
    {
        private readonly StorageOptions storageOptions;

        public BlobService(
            IOptions<StorageOptions> storageOptionsSettings
        )
        {
            this.storageOptions = storageOptionsSettings.Value;
        }

        private BlobContainerClient GetBlobContainerClient(string container)
        {
            //StorageSharedKeyCredential storageCredentials = new StorageSharedKeyCredential(storageOptions.AccountName, storageOptions.AccountKey);
            BlobContainerClient blobContainerClient = new BlobContainerClient(storageOptions.ConnectionString, container);

            return blobContainerClient;
        }

        public async Task DeleteFolderBlobs(string container, string folder)
        {
            var containerClient = GetBlobContainerClient(container);
            var folderBlobs = containerClient.GetBlobsAsync(prefix: folder);
            await foreach (var blobItem in folderBlobs)
            {
                BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);
                await blobClient.DeleteIfExistsAsync();
            }
        }
    }
}
