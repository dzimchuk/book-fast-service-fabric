using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Search;
using BookFast.Search.Client;
using BookFast.Rest;

namespace BookFast.Web.Proxy
{
    internal class SearchProxy : ISearchService
    {
        private readonly ISearchMapper mapper;
        private readonly IApiClientFactory<IBookFastSearchAPI> apiClientFactory;

        public SearchProxy(ISearchMapper mapper, IApiClientFactory<IBookFastSearchAPI> apiClientFactory)
        {
            this.mapper = mapper;
            this.apiClientFactory = apiClientFactory;
        }
        
        public async Task<IList<SearchResult>> SearchAsync(string searchText, int page)
        {
            var api = await apiClientFactory.CreateApiClientAsync();
            var result = await api.SearchWithHttpMessagesAsync(searchText, page);

            return mapper.MapFrom(result.Body);
        }
    }
}