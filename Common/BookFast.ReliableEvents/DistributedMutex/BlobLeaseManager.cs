using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.ReliableEvents.DistributedMutex
{
    /// <summary>
    /// adopted from https://github.com/mspnp/cloud-design-patterns/blob/master/leader-election/DistributedMutex/BlobLeaseManager.cs
    /// </summary>
    internal class BlobLeaseManager
    {
        private readonly CloudPageBlob leaseBlob;
        private readonly ILogger logger;

        public BlobLeaseManager(BlobSettings settings, ILogger logger)
            : this(settings.StorageAccount.CreateCloudBlobClient(), settings.Container, settings.BlobName, logger)
        {
        }

        public BlobLeaseManager(CloudBlobClient blobClient, string leaseContainerName, string leaseBlobName, ILogger logger)
        {
            this.logger = logger;

            var container = blobClient.GetContainerReference(leaseContainerName);
            leaseBlob = container.GetPageBlobReference(leaseBlobName);
        }

        public async Task ReleaseLeaseAsync(string leaseId)
        {
            try
            {
                await leaseBlob.ReleaseLeaseAsync(new AccessCondition { LeaseId = leaseId });
            }
            catch (StorageException storageException)
            {
                logger.LogError($"Error releasing lease. Details: {storageException}");
            }
        }

        public async Task<string> AcquireLeaseAsync(CancellationToken token)
        {
            var blobNotFound = false;

            try
            {
                return await leaseBlob.AcquireLeaseAsync(TimeSpan.FromSeconds(60));
            }
            catch (StorageException storageException)
            {
                var errorCode = storageException.RequestInformation.ExtendedErrorInformation.ErrorCode;
                if (errorCode.Equals("ContainerNotFound", StringComparison.OrdinalIgnoreCase) ||
                    errorCode.Equals("BlobNotFound", StringComparison.OrdinalIgnoreCase))
                {
                    blobNotFound = true;
                }
                else if (errorCode.Equals("LeaseAlreadyPresent", StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
                else
                {
                    logger.LogError($"Error acquiring lease. Details: {storageException}");
                }
            }

            if (blobNotFound)
            {
                await CreateBlobAsync(token);
                return await AcquireLeaseAsync(token);
            }

            return null;
        }

        public async Task<bool> RenewLeaseAsync(string leaseId, CancellationToken token)
        {
            try
            {
                await leaseBlob.RenewLeaseAsync(new AccessCondition { LeaseId = leaseId });
                return true;
            }
            catch (StorageException storageException)
            {
                logger.LogError($"Error renewing lease. Details: {storageException}");
                return false;
            }
        }

        private async Task CreateBlobAsync(CancellationToken token)
        {
            try
            {
                await leaseBlob.Container.CreateIfNotExistsAsync();

                if (!await leaseBlob.ExistsAsync())
                {
                    await leaseBlob.CreateAsync(0);
                }
            }
            catch (StorageException storageException)
            {
                logger.LogError($"Error creating a mutex blob. Details: {storageException}");
                throw;
            }
        }
    }
}
