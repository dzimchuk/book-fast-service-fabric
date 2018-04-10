using BookFast.Booking.Business.Data;
using System;
using System.Threading.Tasks;
using BookFast.Booking.Contracts.Models;
using BookFast.ServiceFabric.Communication;
using BookFast.Facility.Client;

namespace BookFast.Booking.Data
{
    internal class FacilityDataSource : IFacilityDataSource
    {
        private readonly IPartitionClientFactory<CommunicationClient<IBookFastFacilityAPI>> partitionClientFactory;
        private readonly IFacilityMapper mapper;

        public FacilityDataSource(IPartitionClientFactory<CommunicationClient<IBookFastFacilityAPI>> partitionClientFactory, IFacilityMapper mapper)
        {
            this.partitionClientFactory = partitionClientFactory;
            this.mapper = mapper;
        }

        public async Task<Accommodation> FindAccommodationAsync(int accommodationId)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.FindAccommodationWithHttpMessagesAsync(accommodationId);
            });

            return result.Response.StatusCode == System.Net.HttpStatusCode.OK
                ? mapper.MapFrom(result.Body)
                : null;
        }

        public async Task<Contracts.Models.Facility> FindFacilityAsync(int facilityId)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.FindFacilityWithHttpMessagesAsync(facilityId);
            });

            return result.Response.StatusCode == System.Net.HttpStatusCode.OK
                ? mapper.MapFrom(result.Body)
                : null;
        }
    }
}
