using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Web.Contracts.Models;

namespace BookFast.Web.Contracts
{
    public interface IFacilityProxy
    {
        Task<List<Facility>> ListAsync();
        Task<Facility> FindAsync(Guid facilityId);
        Task CreateAsync(FacilityDetails details);
        Task UpdateAsync(Guid facilityId, FacilityDetails details);
        Task DeleteAsync(Guid facilityId);
    }
}