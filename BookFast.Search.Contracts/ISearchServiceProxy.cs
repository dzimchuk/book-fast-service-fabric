using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Search.Contracts.Models;

namespace BookFast.Search.Contracts
{
    public interface ISearchServiceProxy
    {
        Task<IList<SearchResult>> SearchAsync(string searchText, int page);
        Task<IList<SuggestResult>> SuggestAsync(string searchText);
    }
}