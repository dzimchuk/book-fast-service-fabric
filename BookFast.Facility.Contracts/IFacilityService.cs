using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Facility.Contracts.Models;

namespace BookFast.Facility.Contracts
{
    public interface IFacilityService
    {
        Task<List<Models.Facility>> ListAsync();
        Task<Models.Facility> FindAsync(Guid facilityId);
        Task<Models.Facility> CreateAsync(FacilityDetails details);
        Task<Models.Facility> UpdateAsync(Guid facilityId, FacilityDetails details);
        Task DeleteAsync(Guid facilityId);
        Task CheckFacilityAsync(Guid facilityId);
        Task IncrementAccommodationCountAsync(Guid facilityId);
        Task DecrementAccommodationCountAsync(Guid facilityId);
    }
}