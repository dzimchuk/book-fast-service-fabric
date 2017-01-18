using BookFast.Facility.Client.Models;
using BookFast.Files.Contracts.Models;

namespace BookFast.Files.Data
{
    public interface IFacilityMapper
    {
        Accommodation MapFrom(AccommodationRepresentation accommodation);
        Contracts.Models.Facility MapFrom(FacilityRepresentation facility);
    }
}
