using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Web.Contracts.Search;

namespace BookFast.Web.Contracts
{
    public interface ISearchService
    {
        Task<IList<SearchResult>> SearchAsync(string searchText, int page);
    }
}