using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using SearchResult = BookFast.Search.Contracts.Models.SearchResult;
using SuggestResult = BookFast.Search.Contracts.Models.SuggestResult;
using BookFast.Search.Contracts;

namespace BookFast.Search.Adapter
{
    internal class SearchServiceProxy : ISearchServiceProxy
    {
        private const int PageSize = 10;
        private readonly ISearchIndexClient client;
        private readonly ISearchResultMapper mapper;

        public SearchServiceProxy(ISearchIndexClient client, ISearchResultMapper mapper)
        {
            this.client = client;
            this.mapper = mapper;
        }

        public async Task<IList<SearchResult>> SearchAsync(string searchText, int page)
        {
            var parameters = new SearchParameters
            {
                SearchMode = SearchMode.All,
                HighlightFields = new List<string> { "Name", "Description", "FacilityName", "FacilityDescription" },
                HighlightPreTag = "<b>",
                HighlightPostTag = "</b>",
                Skip = (page - 1) * PageSize
            };

            var result = await client.Documents.SearchAsync(searchText, parameters);
            return result.Results != null ? mapper.MapFrom(result.Results) : null;
        }

        public async Task<IList<SuggestResult>> SuggestAsync(string searchText)
        {
            var parameters = new SuggestParameters
            {
                UseFuzzyMatching = true,
                HighlightPreTag = "<b>",
                HighlightPostTag = "</b>"
            };

            var result = await client.Documents.SuggestAsync(searchText, "sg", parameters);
            return result.Results != null ? mapper.MapFrom(result.Results) : null;
        }
    }
}
