using System.Linq;
using BookFast.Facility.Client.Models;
using BookFast.Booking.Domain.Models;

namespace BookFast.Booking.Data.Mappers
{
    internal static class FacilityMapper
    {
        public static Domain.Models.Facility ToDomainModel(this FacilityRepresentation facility) =>
            new Domain.Models.Facility
            {
                Id = facility.Id,
                Name = facility.Name,
                Description = facility.Description,
                StreetAddress = facility.StreetAddress
            };

        public static Accommodation ToDomainModel(this AccommodationRepresentation accommodation) =>
            new Accommodation
            {
                Id = accommodation.Id,
                Name = accommodation.Name,
                Description = accommodation.Description,
                FacilityId = accommodation.FacilityId,
                RoomCount = accommodation.RoomCount,
                Images = accommodation.Images?.ToArray()
            };
    }
}
