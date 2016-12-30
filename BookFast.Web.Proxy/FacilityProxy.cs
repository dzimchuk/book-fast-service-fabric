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
        private readonly IBookFastAPIFactory restClientFactory;
        private readonly IFacilityMapper mapper;
        private readonly ICommunicationClientFactory<HttpCommunicationClient<IBookFastFacilityAPI>> factory;
        private readonly ApiOptions apiOptions;

        public FacilityProxy(IBookFastAPIFactory restClientFactory, IFacilityMapper mapper, 
            ICommunicationClientFactory<HttpCommunicationClient<IBookFastFacilityAPI>> factory, 
            IOptions<ApiOptions> apiOptions)
        {
            this.restClientFactory = restClientFactory;
            this.mapper = mapper;
            this.factory = factory;
            this.apiOptions = apiOptions.Value;
        }

        public async Task<List<Facility>> ListAsync()
        {
            var partitionClient = new ServicePartitionClient<HttpCommunicationClient<IBookFastFacilityAPI>>(factory, new Uri("fabric:/"));
            var result = await partitionClient.InvokeWithRetryAsync(client => client.API.ListFacilitiesWithHttpMessagesAsync());

            return mapper.MapFrom(result.Body);
        }

        public async Task<Facility> FindAsync(Guid facilityId)
        {
            var client = await restClientFactory.CreateAsync();
            var result = await client.FindFacilityWithHttpMessagesAsync(facilityId);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new FacilityNotFoundException(facilityId);

            return mapper.MapFrom(result.Body);
        }

        public async Task CreateAsync(FacilityDetails details)
        {
            var client = await restClientFactory.CreateAsync();
            await client.CreateFacilityWithHttpMessagesAsync(mapper.MapFrom(details));
        }

        public async Task UpdateAsync(Guid facilityId, FacilityDetails details)
        {
            var client = await restClientFactory.CreateAsync();
            var result = await client.UpdateFacilityWithHttpMessagesAsync(facilityId, mapper.MapFrom(details));

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new FacilityNotFoundException(facilityId);
        }

        public async Task DeleteAsync(Guid facilityId)
        {
            var client = await restClientFactory.CreateAsync();
            var result = await client.DeleteFacilityWithHttpMessagesAsync(facilityId);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new FacilityNotFoundException(facilityId);
        }
    }
}