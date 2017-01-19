using System;
using BookFast.Files.Contracts.Models;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using BookFast.Files.Business.Data;

namespace BookFast.Files.Data
{
    internal class SASTokenProvider : ISASTokenProvider
    {
        private readonly AzureStorageOptions storageOptions;

        public SASTokenProvider(IOptions<AzureStorageOptions> storageOptions)
        {
            this.storageOptions = storageOptions.Value;
        }

        public string GetUrlWithAccessToken(string path, AccessPermission permission, DateTimeOffset expirationTime)
        {
            var storageAccount = CloudStorageAccount.Parse(storageOptions.ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(storageOptions.ImageContainer);

            var blob = container.GetBlockBlobReference(path);

            var sasPolicy = new SharedAccessBlobPolicy();
            sasPolicy.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5);
            sasPolicy.SharedAccessExpiryTime = expirationTime;
            sasPolicy.Permissions = MapFrom(permission);
            
            string sasToken = blob.GetSharedAccessSignature(sasPolicy);
            return blob.Uri + sasToken;
        }

        private static SharedAccessBlobPermissions MapFrom(AccessPermission permissions)
        {
            var blobPermissions = SharedAccessBlobPermissions.None;
            if ((permissions & AccessPermission.Read) != 0)
            {
                blobPermissions |= SharedAccessBlobPermissions.Read;
            }

            if ((permissions & AccessPermission.Write) != 0)
            {
                blobPermissions |= SharedAccessBlobPermissions.Write;
            }

            if ((permissions & AccessPermission.Delete) != 0)
            {
                blobPermissions |= SharedAccessBlobPermissions.Delete;
            }

            return blobPermissions;
        }
    }
}
