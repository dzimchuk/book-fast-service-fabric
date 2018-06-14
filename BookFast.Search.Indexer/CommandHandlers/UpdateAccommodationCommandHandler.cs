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
    public class UpdateAccommodationCommandHandler : AsyncRequestHandler<UpdateAccommodationCommand>
    {
        private readonly ISearchIndexer searchIndexer;
        private readonly IApiClientFactory<IBookFastFacilityAPI> apiClientFactory;

        public UpdateAccommodationCommandHandler(ISearchIndexer searchIndexer, IApiClientFactory<IBookFastFacilityAPI> apiClientFactory)
        {
            this.searchIndexer = searchIndexer;
            this.apiClientFactory = apiClientFactory;
        }

        protected override async Task Handle(UpdateAccommodationCommand request, CancellationToken cancellationToken)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.FindFacilityWithHttpMessagesAsync(request.FacilityId);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            var accommodation = new Accommodation
            {
                Id = request.Id,
                FacilityId = request.FacilityId,
                Name = request.Name,
                Description = request.Description,
                RoomCount = request.RoomCount,
                Images = request.Images,
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
