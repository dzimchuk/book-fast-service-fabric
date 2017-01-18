using BookFast.Booking.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace BookFast.Booking.Business.Data
{
    public interface IFacilityProxy
    {
        Task<Accommodation> FindAccommodationAsync(Guid accommodationId);
        Task<Facility> FindFacilityAsync(Guid facilityId);
    }
}
