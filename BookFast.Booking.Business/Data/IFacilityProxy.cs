using BookFast.Booking.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.Booking.Business.Data
{
    public interface IFacilityProxy
    {
        Task<List<Facility>> ListFacilitiesAsync(CancellationToken cancellationToken);
        Task<List<Accommodation>> ListAccommodationsAsync(Guid facilityId, CancellationToken cancellationToken);
    }
}
