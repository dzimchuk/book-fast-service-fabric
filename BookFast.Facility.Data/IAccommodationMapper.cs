using System.Collections.Generic;
using BookFast.Facility.Contracts.Models;

namespace BookFast.Facility.Data
{
    public interface IAccommodationMapper
    {
        Accommodation MapFrom(Models.Accommodation accommodation);
        IEnumerable<Accommodation> MapFrom(IEnumerable<Models.Accommodation> accommodations);
        Models.Accommodation MapFrom(Accommodation accommodation);
    }
}