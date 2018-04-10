using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Web.Contracts.Models;

namespace BookFast.Web.Contracts
{
    public interface IFacilityProxy
    {
        Task<List<Facility>> ListAsync();
        Task<Facility> FindAsync(int facilityId);
        Task CreateAsync(FacilityDetails details);
        Task UpdateAsync(int facilityId, FacilityDetails details);
        Task DeleteAsync(int facilityId);
    }
}