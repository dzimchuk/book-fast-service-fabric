using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using BookFast.Web.Contracts.Models;
using BookFast.ServiceFabric.Communication;
using BookFast.Facility.Client;

namespace BookFast.Web.Proxy
{
    internal class FacilityProxy : IFacilityService
    {
        private readonly IFacilityMapper mapper;
        private readonly IPartitionClientFactory<CommunicationClient<IBookFastFacilityAPI>> partitionClientFactory;

        public FacilityProxy(IFacilityMapper mapper,
            IPartitionClientFactory<CommunicationClient<IBookFastFacilityAPI>> partitionClientFactory)
        {
            this.mapper = mapper;
            this.partitionClientFactory = partitionClientFactory;
        }
                
        public async Task<List<Contracts.Models.Facility>> ListAsync()
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.ListFacilitiesWithHttpMessagesAsync();
            });

            return mapper.MapFrom(result.Body);
        }

        public async Task<Contracts.Models.Facility> FindAsync(Guid facilityId)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.FindFacilityWithHttpMessagesAsync(facilityId);
            });

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FacilityNotFoundException(facilityId);
            }

            return mapper.MapFrom(result.Body);
        }

        public Task CreateAsync(FacilityDetails details)
        {
            return partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.CreateFacilityWithHttpMessagesAsync(mapper.MapFrom(details));
            });
        }

        public async Task UpdateAsync(Guid facilityId, FacilityDetails details)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.UpdateFacilityWithHttpMessagesAsync(facilityId, mapper.MapFrom(details));
            });

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FacilityNotFoundException(facilityId);
            }
        }

        public async Task DeleteAsync(Guid facilityId)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(async client =>
            {
                var api = await client.CreateApiClient();
                return await api.DeleteFacilityWithHttpMessagesAsync(facilityId);
            });

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new FacilityNotFoundException(facilityId);
            }
        }
    }
}