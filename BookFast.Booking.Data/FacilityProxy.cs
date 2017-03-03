using BookFast.Booking.Business.Data;
using System;
using System.Threading.Tasks;
using BookFast.Booking.Contracts.Models;
using BookFast.ServiceFabric.Communication;
using BookFast.Facility.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BookFast.Booking.Data
{
    internal class FacilityProxy : IFacilityProxy
    {
        private readonly IPartitionClientFactory<CommunicationClient<IBookFastFacilityAPI>> partitionClientFactory;
        private readonly IFacilityMapper mapper;

        public FacilityProxy(IPartitionClientFactory<CommunicationClient<IBookFastFacilityAPI>> partitionClientFactory, IFacilityMapper mapper)
        {
            this.partitionClientFactory = partitionClientFactory;
            this.mapper = mapper;
        }

        public async Task<List<Contracts.Models.Facility>> ListFacilitiesAsync(CancellationToken cancellationToken)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.ListFacilitiesWithHttpMessagesAsync(cancellationToken: cancellationToken);
            }, cancellationToken);

            return result.Response.StatusCode == System.Net.HttpStatusCode.OK
                ? result.Body.Select(facility => mapper.MapFrom(facility)).ToList()
                : new List<Contracts.Models.Facility>();
        }

        public async Task<List<Accommodation>> ListAccommodationsAsync(Guid facilityId, CancellationToken cancellationToken)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.ListAccommodationsWithHttpMessagesAsync(facilityId, cancellationToken: cancellationToken);
            }, cancellationToken);

            return result.Response.StatusCode == System.Net.HttpStatusCode.OK
                ? result.Body.Select(accommodation => mapper.MapFrom(accommodation)).ToList()
                : new List<Accommodation>();
        }
    }
}
