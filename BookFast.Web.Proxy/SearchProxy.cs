using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Search;
using Microsoft.ServiceFabric.Services.Communication.Client;
using BookFast.Web.Proxy.RestClient;
using Microsoft.Extensions.Options;
using System;

namespace BookFast.Web.Proxy
{
    internal class SearchProxy : ISearchService
    {
        private readonly ICommunicationClientFactory<CommunicationClient<IBookFastSearchAPI>> clientFactory;
        private readonly ISearchMapper mapper;
        private readonly ApiOptions apiOptions;

        public SearchProxy(ICommunicationClientFactory<CommunicationClient<IBookFastSearchAPI>> clientFactory, ISearchMapper mapper, IOptions<ApiOptions> apiOptions)
        {
            this.clientFactory = clientFactory;
            this.mapper = mapper;
            this.apiOptions = apiOptions.Value;
        }

        private ServicePartitionClient<CommunicationClient<IBookFastSearchAPI>> PartitionClient =>
            new ServicePartitionClient<CommunicationClient<IBookFastSearchAPI>>(clientFactory, new Uri(apiOptions.SearchService));

        public async Task<IList<SearchResult>> SearchAsync(string searchText, int page)
        {
            var result = await PartitionClient.InvokeWithRetryAsync(client => client.API.SearchWithHttpMessagesAsync(searchText, page));
            return mapper.MapFrom(result.Body);
        }
    }
}