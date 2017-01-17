using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using BookFast.Web.Contracts.Models;
using Microsoft.ServiceFabric.Services.Communication.Client;
using BookFast.Web.Proxy.RestClient;
using Microsoft.Extensions.Options;

namespace BookFast.Web.Proxy
{
    internal class FacilityProxy : IFacilityService
    {
        private readonly IFacilityMapper mapper;
        private readonly ICommunicationClientFactory<CommunicationClient<IBookFastFacilityAPI>> factory;
        private readonly ApiOptions apiOptions;

        public FacilityProxy(IFacilityMapper mapper, 
            ICommunicationClientFactory<CommunicationClient<IBookFastFacilityAPI>> factory, 
            IOptions<ApiOptions> apiOptions)
        {
            this.mapper = mapper;
            this.factory = factory;
            this.apiOptions = apiOptions.Value;
        }

        private ServicePartitionClient<CommunicationClient<IBookFastFacilityAPI>> PartitionClient =>
            new ServicePartitionClient<CommunicationClient<IBookFastFacilityAPI>>(factory, new Uri(apiOptions.FacilityService));
        
        public async Task<List<Facility>> ListAsync()
        {
            var result = await PartitionClient.InvokeWithRetryAsync(client => client.API.ListFacilitiesWithHttpMessagesAsync());
            return mapper.MapFrom(result.Body);
        }

        public async Task<Facility> FindAsync(Guid facilityId)
        {
            var result = await PartitionClient.InvokeWithRetryAsync(client => client.API.FindFacilityWithHttpMessagesAsync(facilityId));

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new FacilityNotFoundException(facilityId);

            return mapper.MapFrom(result.Body);
        }

        public Task CreateAsync(FacilityDetails details)
        {
            return PartitionClient.InvokeWithRetryAsync(client => client.API.CreateFacilityWithHttpMessagesAsync(mapper.MapFrom(details)));
        }

        public async Task UpdateAsync(Guid facilityId, FacilityDetails details)
        {
            var result = await PartitionClient.InvokeWithRetryAsync(client => client.API.UpdateFacilityWithHttpMessagesAsync(facilityId, mapper.MapFrom(details)));

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new FacilityNotFoundException(facilityId);
        }

        public async Task DeleteAsync(Guid facilityId)
        {
            var result = await PartitionClient.InvokeWithRetryAsync(client => client.API.DeleteFacilityWithHttpMessagesAsync(facilityId));

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new FacilityNotFoundException(facilityId);
        }
    }
}