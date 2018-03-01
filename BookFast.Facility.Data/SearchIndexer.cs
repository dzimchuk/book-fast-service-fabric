using BookFast.Facility.Business;
using System;
using System.Threading.Tasks;
using BookFast.Facility.Contracts.Models;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using BookFast.Facility.Data.Commands;

namespace BookFast.Facility.Data
{
    internal class SearchIndexer : ISearchIndexer
    {
        private readonly CloudQueue queue;

        public SearchIndexer(IOptions<SearchQueueOptions> options)
        {
            var storageAccount = CloudStorageAccount.Parse(options.Value.ConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();

            queue = queueClient.GetQueueReference(options.Value.SearchIndexQueueName);
            queue.CreateIfNotExistsAsync().Wait(); // todo: move resource initialization to service startup
        }

        public Task DeleteAccommodationIndexAsync(Guid accommodationId)
        {
            var command = new DeleteAccommodationSearchIndexCommand(accommodationId);
            return command.ApplyAsync(queue);
        }

        public Task IndexAccommodationAsync(Accommodation accommodation, Contracts.Models.Facility facility)
        {
            var command = new UpdateAccommodationSearchIndexCommand(accommodation, facility);
            return command.ApplyAsync(queue);
        }
    }
}
