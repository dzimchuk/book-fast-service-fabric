using BookFast.Search.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace BookFast.Search.Contracts
{
    public interface ISearchIndexer
    {
        Task IndexAccommodationAsync(Accommodation accommodation);
        Task DeleteAccommodationIndexAsync(Guid accommodationId);
    }
}
