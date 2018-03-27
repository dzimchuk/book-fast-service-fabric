using BookFast.Facility.Data.Models;
using BookFast.Facility.QueryStack;
using BookFast.Facility.QueryStack.Representations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFast.Facility.Data.Queries
{
    internal class AccommodationQueryDataSource : IAccommodationQueryDataSource
    {
        private readonly FacilityContext context;

        public AccommodationQueryDataSource(FacilityContext context)
        {
            this.context = context;
        }

        public async Task<AccommodationRepresentation> FindAsync(int id)
        {
            var accommodation = await context.Accommodations.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);
            return accommodation != null ? ToRepresentation(accommodation) : null;
        }

        public async Task<IEnumerable<AccommodationRepresentation>> ListAsync(int facilityId)
        {
            var accommodations = await context.Accommodations.AsNoTracking().Where(item => item.FacilityId == facilityId).ToListAsync();
            return accommodations.Select(item => ToRepresentation(item)).ToList();
        }

        private static AccommodationRepresentation ToRepresentation(Accommodation accommodation)
        {
            return new AccommodationRepresentation
            {
                Id = accommodation.Id,
                FacilityId = accommodation.FacilityId,
                Name = accommodation.Name,
                Description = accommodation.Description,
                RoomCount = accommodation.RoomCount,
                Images = accommodation.Images.ToStringArray()
            };
        }
    }
}
