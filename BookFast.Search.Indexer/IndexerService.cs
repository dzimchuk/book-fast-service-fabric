using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Runtime;
using BookFast.Search.Contracts;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using BookFast.Search.Contracts.Models;
using Newtonsoft.Json;

namespace BookFast.Search.Indexer
{
    internal sealed class IndexerService : StatelessService
    {
        private readonly ISearchIndexer searchIndexer;
        private readonly CloudQueue queue;

        public IndexerService(StatelessServiceContext context, ISearchIndexer searchIndexer, IOptions<QueueOptions> options)
            : base(context)
        {
            this.searchIndexer = searchIndexer;

            var storageAccount = CloudStorageAccount.Parse(options.Value.ConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();

            queue = queueClient.GetQueueReference(options.Value.SearchIndexQueueName);
            queue.CreateIfNotExists();
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var message = await queue.GetMessageAsync(cancellationToken);
                if (message != null)
                {
                    ServiceEventSource.Current.ServiceMessage(Context, "Processing message {0}.", message.Id);
                    await ProcessMessageAsync(message);
                    await DeleteMessageAsync(message);
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                }
            }
        }

        private async Task ProcessMessageAsync(CloudQueueMessage message)
        {
            var messageContent = Deserialize(message);
            if (messageContent == null)
            {
                return;
            }

            try
            {
                if ("update".Equals(messageContent.Action, StringComparison.OrdinalIgnoreCase))
                {
                    await searchIndexer.IndexAccommodationAsync(messageContent.Accommodation);
                }
                else if ("delete".Equals(messageContent.Action, StringComparison.OrdinalIgnoreCase) && messageContent.Accommodation != null)
                {
                    await searchIndexer.DeleteAccommodationIndexAsync(messageContent.Accommodation.Id);
                }
                else
                {
                    ServiceEventSource.Current.ServiceMessage(Context, "Unknown action {0}.", messageContent.Action);
                }
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(Context, "Error processing message {0}. Details: {1}", message.Id, ex.ToString());
            }
        }

        private async Task DeleteMessageAsync(CloudQueueMessage message)
        {
            try
            {
                await queue.DeleteMessageAsync(message);
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.ExtendedErrorInformation.ErrorCode == "MessageNotFound")
                {
                    ServiceEventSource.Current.ServiceMessage(Context, "Message {0} not found. It might have been reprocessed and removed by another instance.", message.Id);
                }
                else
                {
                    ServiceEventSource.Current.ServiceMessage(Context, "Failed to delete message {0}. Details: {1}", message.Id, ex.ToString());
                }
            }
        }

        private Message Deserialize(CloudQueueMessage message)
        {
            try
            {
                return JsonConvert.DeserializeObject<Message>(message.AsString);
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(Context, "Failed to deserialize message {0}. Details: {1}", message.Id, ex.ToString());
                return null;
            }
        }

        private class Message
        {
            public string Action { get; set; }
            public Accommodation Accommodation { get; set; }
        }
    }
}