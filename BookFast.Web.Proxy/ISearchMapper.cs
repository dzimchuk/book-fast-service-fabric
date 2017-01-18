using System.Collections.Generic;
using BookFast.Search.Client.Models;

namespace BookFast.Web.Proxy
{
    public interface ISearchMapper
    {
        IList<Contracts.Search.SearchResult> MapFrom(IList<SearchResult> results);
    }
}