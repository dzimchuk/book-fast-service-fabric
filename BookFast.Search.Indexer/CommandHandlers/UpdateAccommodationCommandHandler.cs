using BookFast.Facility.Client;
using BookFast.Rest;
using BookFast.Search.Contracts;
using BookFast.Search.Contracts.Models;
using BookFast.Search.Indexer.Commands;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Search.Indexer.CommandHandlers
{
    public class UpdateAccommodationCommandHandler : IRequestHandler<UpdateAccommodationCommand>
    {
        private readonly ISearchIndexer searchIndexer;
        private readonly IApiClientFactory<IBookFastFacilityAPI> apiClientFactory;

        public UpdateAccommodationCommandHandler(ISearchIndexer searchIndexer, IApiClientFactory<IBookFastFacilityAPI> apiClientFactory)
        {
            this.searchIndexer = searchIndexer;
            this.apiClientFactory = apiClientFactory;
        }

        public async Task Handle(UpdateAccommodationCommand message, CancellationToken cancellationToken)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.FindFacilityWithHttpMessagesAsync(message.FacilityId);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            var accommodation = new Accommodation
            {
                Id = message.Id,
                FacilityId = message.FacilityId,
                Name = message.Name,
                Description = message.Description,
                RoomCount = message.RoomCount,
                Images = message.Images,
                FacilityName = result.Body.Name,
                FacilityDescription = result.Body.Description,
                FacilityLocation = new Location
                {
                    Latitude = result.Body.Latitude,
                    Longitude = result.Body.Longitude
                }
            };

            await searchIndexer.IndexAccommodationAsync(accommodation);
        }
    }
}
