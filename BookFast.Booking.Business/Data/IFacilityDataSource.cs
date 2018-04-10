using BookFast.Booking.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace BookFast.Booking.Business.Data
{
    public interface IFacilityDataSource
    {
        Task<Accommodation> FindAccommodationAsync(int accommodationId);
        Task<Facility> FindFacilityAsync(int facilityId);
    }
}
