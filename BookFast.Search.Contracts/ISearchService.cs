using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Search.Contracts.Models;

namespace BookFast.Search.Contracts
{
    public interface ISearchService
    {
        Task<IList<SearchResult>> SearchAsync(string searchText, int page);
        Task<IList<SuggestResult>> SuggestAsync(string searchText);
    }
}