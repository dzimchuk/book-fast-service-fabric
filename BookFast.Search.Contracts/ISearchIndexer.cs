using BookFast.Search.Contracts.Models;
using System.Threading.Tasks;

namespace BookFast.Search.Contracts
{
    public interface ISearchIndexer
    {
        Task IndexAccommodationAsync(Accommodation accommodation);
        Task DeleteAccommodationIndexAsync(int accommodationId);
    }
}
