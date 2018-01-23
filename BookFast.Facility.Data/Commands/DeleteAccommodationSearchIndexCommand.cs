using BookFast.SeedWork;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.Facility.Data.Commands
{
    internal class DeleteAccommodationSearchIndexCommand : ICommand<CloudQueue>
    {
        private readonly Guid accommodationId;

        public DeleteAccommodationSearchIndexCommand(Guid accommodationId)
        {
            this.accommodationId = accommodationId;
        }

        public Task ApplyAsync(CloudQueue model)
        {
            var payload = new Dictionary<string, object>
            {
                { "Action", "delete" },
                { "Accommodation", new { Id = accommodationId } }
            };

            var message = new CloudQueueMessage(JsonConvert.SerializeObject(payload));
            return model.AddMessageAsync(message);
        }
    }
}
