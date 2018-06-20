using Microsoft.WindowsAzure.Storage;

namespace BookFast.ReliableEvents.DistributedMutex
{
    /// <summary>
    /// adopted from https://github.com/mspnp/cloud-design-patterns/blob/master/leader-election/DistributedMutex/BlobLeaseManager.cs
    /// </summary>
    internal struct BlobSettings
    {
        public string Container { get; }
        public string BlobName { get; }
        public CloudStorageAccount StorageAccount { get; set; }

        public BlobSettings(CloudStorageAccount storageAccount, string container, string blobName)
        {
            this.StorageAccount = storageAccount;
            this.Container = container;
            this.BlobName = blobName;
        }
    }
}
