using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Search;
using BookFast.ServiceFabric.Communication;
using BookFast.Search.Client;

namespace BookFast.Web.Proxy
{
    internal class SearchProxy : ISearchService
    {
        private readonly IPartitionClientFactory<CommunicationClient<IBookFastSearchAPI>> partitionClientFactory;
        private readonly ISearchMapper mapper;

        public SearchProxy(IPartitionClientFactory<CommunicationClient<IBookFastSearchAPI>> partitionClientFactory, ISearchMapper mapper)
        {
            this.mapper = mapper;
            this.partitionClientFactory = partitionClientFactory;
        }
        
        public async Task<IList<SearchResult>> SearchAsync(string searchText, int page)
        {
            var result = await partitionClientFactory.CreatePartitionClient().InvokeWithRetryAsync(client => client.API.SearchWithHttpMessagesAsync(searchText, page));
            return mapper.MapFrom(result.Body);
        }
    }
}