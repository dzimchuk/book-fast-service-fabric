using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Web.Contracts.Search;

namespace BookFast.Web.Contracts
{
    public interface ISearchProxy
    {
        Task<IList<SearchResult>> SearchAsync(string searchText, int page);
    }
}