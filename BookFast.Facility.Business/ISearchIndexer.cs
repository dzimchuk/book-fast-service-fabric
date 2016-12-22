using System;
using System.Threading.Tasks;
using BookFast.Facility.Contracts.Models;

namespace BookFast.Facility.Business
{
    public interface ISearchIndexer
    {
        Task IndexAccommodationAsync(Accommodation accommodation, Contracts.Models.Facility facility);
        Task DeleteAccommodationIndexAsync(Guid accommodationId);
    }
}