using BookFast.Files.Business.Data;
using System;
using System.Threading.Tasks;
using BookFast.Files.Contracts.Models;
using BookFast.ServiceFabric.Communication;
using BookFast.Facility.Client;

namespace BookFast.Files.Data
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

        public async Task<Accommodation> FindAccommodationAsync(Guid accommodationId)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(client => client.API.FindAccommodationWithHttpMessagesAsync(accommodationId));
            return result.Response.StatusCode == System.Net.HttpStatusCode.OK ? mapper.MapFrom(result.Body) : null;
        }

        public async Task<Contracts.Models.Facility> FindFacilityAsync(Guid facilityId)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(client => client.API.FindFacilityWithHttpMessagesAsync(facilityId));
            return result.Response.StatusCode == System.Net.HttpStatusCode.OK ? mapper.MapFrom(result.Body) : null;
        }
    }
}
