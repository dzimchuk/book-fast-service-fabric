using BookFast.Facility.CommandStack.Repositories;
using BookFast.Facility.Domain.Models;
using BookFast.Security;
using System.Threading.Tasks;

namespace BookFast.Facility.Data.Repositories
{
    internal class AccommodationRepository : IAccommodationRepository
    {
        private readonly FacilityContext context;
        private readonly ISecurityContext securityContext;

        public AccommodationRepository(FacilityContext context, ISecurityContext securityContext)
        {
            this.context = context;
            this.securityContext = securityContext;
        }

        public async Task<int> AddAsync(Accommodation accommodation)
        {
            var dataModel = new Models.Accommodation
            {
                FacilityId = accommodation.FacilityId,
                Name = accommodation.Name,
                Description = accommodation.Description,
                RoomCount = accommodation.RoomCount,
                Images = accommodation.Images.ToJson()
            };

            await context.Accommodations.AddAsync(dataModel);

            return dataModel.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var trackedAccommodation = await context.Accommodations.FindAsync(id);
            context.Accommodations.Remove(trackedAccommodation);
        }

        public async Task<Accommodation> FindAsync(int id)
        {
            var accommodation = await context.Accommodations.FindAsync(id);
            return accommodation != null
                ? Accommodation.FromStorage(
                    accommodation.Id,
                    accommodation.FacilityId,
                    accommodation.Name,
                    accommodation.Description,
                    accommodation.RoomCount,
                    accommodation.Images.ToStringArray())
                : null;
        }

        public Task PersistEventsAsync(Accommodation entity)
        {
            return context.PersistEventsAsync(entity, securityContext);
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Accommodation accommodation)
        {
            var trackedAccommodation = await context.Accommodations.FindAsync(accommodation.Id);

            trackedAccommodation.Name = accommodation.Name;
            trackedAccommodation.Description = accommodation.Description;
            trackedAccommodation.RoomCount = accommodation.RoomCount;
            trackedAccommodation.Images = accommodation.Images.ToJson();
        }
    }
}
