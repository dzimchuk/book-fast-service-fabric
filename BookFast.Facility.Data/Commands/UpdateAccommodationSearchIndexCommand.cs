using BookFast.Facility.Contracts.Models;
using BookFast.Framework;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.Facility.Data.Commands
{
    internal class UpdateAccommodationSearchIndexCommand : ICommand<CloudQueue>
    {
        private readonly Accommodation accommodation;
        private readonly Contracts.Models.Facility facility;

        public UpdateAccommodationSearchIndexCommand(Accommodation accommodation, Contracts.Models.Facility facility)
        {
            this.accommodation = accommodation;
            this.facility = facility;
        }

        public Task ApplyAsync(CloudQueue model)
        {
            var payload = new Dictionary<string, object>
            {
                { "command", "update" },
                { "Id", accommodation.Id },
                { "FacilityId", accommodation.FacilityId },
                { "Name", accommodation.Details.Name },
                { "Description", accommodation.Details.Description },
                { "FacilityName", facility.Details.Name },
                { "FacilityDescription", facility.Details.Description },
                { "Location", facility.Location },
                { "RoomCount", accommodation.Details.RoomCount },
                { "Images", accommodation.Details.Images }
            };

            var message = new CloudQueueMessage(JsonConvert.SerializeObject(payload));
            return model.AddMessageAsync(message);
        }
    }
}
