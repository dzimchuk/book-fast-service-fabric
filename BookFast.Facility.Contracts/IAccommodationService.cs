using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Facility.Contracts.Models;

namespace BookFast.Facility.Contracts
{
    public interface IAccommodationService
    {
        Task<List<Accommodation>> ListAsync(Guid facilityId);
        Task<Accommodation> FindAsync(Guid accommodationId);
        Task<Accommodation> CreateAsync(Guid facilityId, AccommodationDetails details);
        Task<Accommodation> UpdateAsync(Guid accommodationId, AccommodationDetails details);
        Task DeleteAsync(Guid accommodationId);
        Task CheckAccommodationAsync(Guid accommodationId);
    }
}