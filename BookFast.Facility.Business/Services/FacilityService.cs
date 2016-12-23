using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Facility.Business.Data;
using BookFast.Facility.Contracts;
using BookFast.Facility.Contracts.Exceptions;
using BookFast.Facility.Contracts.Models;
using BookFast.Security;

namespace BookFast.Facility.Business.Services
{
    internal class FacilityService : IFacilityService
    {
        private readonly IFacilityDataSource dataSource;
        private readonly ISecurityContext securityContext;

        public FacilityService(IFacilityDataSource dataSource, ISecurityContext securityContext)
        {
            this.dataSource = dataSource;
            this.securityContext = securityContext;
        }

        public Task<List<Contracts.Models.Facility>> ListAsync()
        {
            return dataSource.ListByOwnerAsync(securityContext.GetCurrentTenant());
        }

        public async Task<Contracts.Models.Facility> FindAsync(Guid facilityId)
        {
            var facility = await dataSource.FindAsync(facilityId);
            if (facility == null)
                throw new FacilityNotFoundException(facilityId);

            return facility;
        }

        public async Task<Contracts.Models.Facility> CreateAsync(FacilityDetails details)
        {
            details.Images = ImagePathHelper.CleanUp(details.Images);

            var facility = new Contracts.Models.Facility
            {
                Id = Guid.NewGuid(),
                Details = details,
                Location = new Location(),
                Owner = securityContext.GetCurrentTenant()
            };
            
            await dataSource.CreateAsync(facility);
            return facility;
        }
        
        public async Task<Contracts.Models.Facility> UpdateAsync(Guid facilityId, FacilityDetails details)
        {
            var facility = await dataSource.FindAsync(facilityId);
            if (facility == null)
                throw new FacilityNotFoundException(facilityId);

            details.Images = ImagePathHelper.Merge(facility.Details.Images, details.Images);
            facility.Details = details;
            await dataSource.UpdateAsync(facility);

            return facility;
        }

        public async Task DeleteAsync(Guid facilityId)
        {
            if (!await dataSource.ExistsAsync(facilityId))
                throw new FacilityNotFoundException(facilityId);

            await dataSource.DeleteAsync(facilityId);
        }

        public async Task CheckFacilityAsync(Guid facilityId)
        {
            if (!await dataSource.ExistsAsync(facilityId))
                throw new FacilityNotFoundException(facilityId);
        }

        public async Task IncrementAccommodationCountAsync(Guid facilityId)
        {
            var facility = await FindAsync(facilityId);

            facility.AccommodationCount++;
            await dataSource.UpdateAsync(facility);
        }

        public async Task DecrementAccommodationCountAsync(Guid facilityId)
        {
            var facility = await FindAsync(facilityId);

            facility.AccommodationCount--;
            await dataSource.UpdateAsync(facility);
        }
    }
}