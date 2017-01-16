using System.Collections.Generic;
using BookFast.Search.Contracts.Models;

namespace BookFast.Search.Adapter
{
    public interface ISearchResultMapper
    {
        IList<SearchResult> MapFrom(IList<Microsoft.Azure.Search.Models.SearchResult> results);
        IList<SuggestResult> MapFrom(IList<Microsoft.Azure.Search.Models.SuggestResult> results);

    }
}