using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Facility.Business.Data;
using BookFast.Facility.Contracts;
using BookFast.Facility.Contracts.Models;
using BookFast.Facility.Contracts.Exceptions;

namespace BookFast.Facility.Business.Services
{
    internal class AccommodationService : IAccommodationService
    {
        private readonly IAccommodationDataSource accommodationDataSource;
        private readonly IFacilityService facilityService;
        private readonly ISearchIndexer searchIndexer;

        public AccommodationService(IAccommodationDataSource accommodationDataSource, IFacilityService facilityService, ISearchIndexer searchIndexer)
        {
            this.accommodationDataSource = accommodationDataSource;
            this.facilityService = facilityService;
            this.searchIndexer = searchIndexer;
        }

        public async Task<List<Accommodation>> ListAsync(Guid facilityId)
        {
            await facilityService.CheckFacilityAsync(facilityId);
            return await accommodationDataSource.ListAsync(facilityId);
        }

        public async Task<Accommodation> FindAsync(Guid accommodationId)
        {
            var accommodation = await accommodationDataSource.FindAsync(accommodationId);
            if (accommodation == null)
                throw new AccommodationNotFoundException(accommodationId);

            return accommodation;
        }

        public async Task<Accommodation> CreateAsync(Guid facilityId, AccommodationDetails details)
        {
            await facilityService.CheckFacilityAsync(facilityId);

            details.Images = ImagePathHelper.CleanUp(details.Images);
            var accommodation = new Accommodation
                                {
                                    Id = Guid.NewGuid(),
                                    FacilityId = facilityId,
                                    Details = details
                                };

            await accommodationDataSource.CreateAsync(accommodation);
            await facilityService.IncrementAccommodationCountAsync(facilityId);

            var facility = await facilityService.FindAsync(facilityId);
            await searchIndexer.IndexAccommodationAsync(accommodation, facility);

            return accommodation;
        }

        public async Task<Accommodation> UpdateAsync(Guid accommodationId, AccommodationDetails details)
        {
            var accommodation = await accommodationDataSource.FindAsync(accommodationId);
            if (accommodation == null)
                throw new AccommodationNotFoundException(accommodationId);

            var facility = await facilityService.FindAsync(accommodation.FacilityId);
            if (facility == null)
                throw new FacilityNotFoundException(accommodation.FacilityId);

            details.Images = ImagePathHelper.Merge(accommodation.Details.Images, details.Images);
            accommodation.Details = details;
            await accommodationDataSource.UpdateAsync(accommodation);

            await searchIndexer.IndexAccommodationAsync(accommodation, facility);

            return accommodation;
        }

        public async Task DeleteAsync(Guid accommodationId)
        {
            var accommodation = await accommodationDataSource.FindAsync(accommodationId);
            if (accommodation == null)
                throw new AccommodationNotFoundException(accommodationId);
            
            await accommodationDataSource.DeleteAsync(accommodationId);
            await facilityService.DecrementAccommodationCountAsync(accommodation.FacilityId);

            await searchIndexer.DeleteAccommodationIndexAsync(accommodationId);
        }

        public async Task CheckAccommodationAsync(Guid accommodationId)
        {
            if (!await accommodationDataSource.ExistsAsync(accommodationId))
                throw new AccommodationNotFoundException(accommodationId);
        }
    }
}