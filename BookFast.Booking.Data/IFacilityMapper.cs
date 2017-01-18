using BookFast.Booking.Contracts.Models;
using BookFast.Facility.Client.Models;

namespace BookFast.Booking.Data
{
    public interface IFacilityMapper
    {
        Accommodation MapFrom(AccommodationRepresentation accommodation);
        Contracts.Models.Facility MapFrom(FacilityRepresentation facility);
    }
}
