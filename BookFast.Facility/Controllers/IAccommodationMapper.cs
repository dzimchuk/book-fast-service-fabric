using System.Collections.Generic;
using BookFast.Facility.Models;
using BookFast.Facility.Models.Representations;
using BookFast.Facility.Contracts.Models;

namespace BookFast.Facility.Controllers
{
    public interface IAccommodationMapper
    {
        AccommodationRepresentation MapFrom(Accommodation accommodation);
        IEnumerable<AccommodationRepresentation> MapFrom(IEnumerable<Accommodation> accommodations);
        AccommodationDetails MapFrom(AccommodationData data);
    }
}