using BookFast.Booking.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.Booking.Business.Data
{
    public interface IFacilityDataSource
    {
        Task<Accommodation> FindAccommodationAsync(Guid accommodationId);
        Task<Facility> FindFacilityAsync(Guid facilityId);
        Task UpdateAccommodationsAsync(List<Accommodation> accommodations);
        Task UpdateFacilitiesAsync(List<Facility> facilities);
    }
}
