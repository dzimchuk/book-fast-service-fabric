using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.Facility.Business.Data
{
    public interface IFacilityDataSource
    {
        Task<List<Contracts.Models.Facility>> ListByOwnerAsync(string owner);
        Task<Contracts.Models.Facility> FindAsync(Guid facilityId);
        Task CreateAsync(Contracts.Models.Facility facility);
        Task UpdateAsync(Contracts.Models.Facility facility);
        Task<bool> ExistsAsync(Guid facilityId);
        Task DeleteAsync(Guid facilityId);
    }
}