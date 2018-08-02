using System.Threading.Tasks;
using BookFast.ServiceFabric.Communication;
using BookFast.Facility.Client;
using BookFast.Booking.CommandStack.Data;
using BookFast.Booking.Domain.Models;
using BookFast.Booking.Data.Mappers;

namespace BookFast.Booking.Data
{
    internal class FacilityDataSource : IFacilityDataSource
    {
        private readonly IPartitionClientFactory<CommunicationClient<IBookFastFacilityAPI>> partitionClientFactory;

        public FacilityDataSource(IPartitionClientFactory<CommunicationClient<IBookFastFacilityAPI>> partitionClientFactory)
        {
            this.partitionClientFactory = partitionClientFactory;
        }

        public async Task<Accommodation> FindAccommodationAsync(int accommodationId)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.FindAccommodationWithHttpMessagesAsync(accommodationId);
            });

            return result.Response.StatusCode == System.Net.HttpStatusCode.OK
                ? result.Body.ToDomainModel()
                : null;
        }

        public async Task<Domain.Models.Facility> FindFacilityAsync(int facilityId)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.FindFacilityWithHttpMessagesAsync(facilityId);
            });

            return result.Response.StatusCode == System.Net.HttpStatusCode.OK
                ? result.Body.ToDomainModel()
                : null;
        }
    }
}
