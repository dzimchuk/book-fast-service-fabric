using BookFast.Facility.CommandStack.Repositories;
using BookFast.ReliableEvents;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookFast.Facility.Data.Repositories
{
    internal class FacilityRepository : IFacilityRepository
    {
        private readonly FacilityContext context;

        public FacilityRepository(FacilityContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Domain.Models.Facility facility)
        {
            var dataModel = new Models.Facility
            {
                Owner = facility.Owner,
                Name = facility.Name,
                Description = facility.Description,
                StreetAddress = facility.StreetAddress,
                Latitude = facility.Location?.Latitude,
                Longitude = facility.Location?.Longitude,
                Images = facility.Images.ToJson()
            };

            await context.Facilities.AddAsync(dataModel);

            return dataModel.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var trackedFacility = await context.Facilities.FindAsync(id);
            context.Facilities.Remove(trackedFacility);
        }

        public async Task<Domain.Models.Facility> FindAsync(int id)
        {
            var facility = await context.Facilities.Include(item => item.Accommodations).FirstOrDefaultAsync(item => item.Id == id);
            return facility != null
                ? Domain.Models.Facility.FromStorage(
                    facility.Id,
                    facility.Owner,
                    facility.Name,
                    facility.Description,
                    facility.StreetAddress,
                    facility.Latitude,
                    facility.Longitude,
                    facility.Images.ToStringArray(),
                    facility.Accommodations.Count)
                : null;
        }

        public Task PersistEventsAsync(IEnumerable<ReliableEvent> events)
        {
            context.Events.AddRange(events);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Domain.Models.Facility facility)
        {
            var trackedFacility = await context.Facilities.FindAsync(facility.Id);

            trackedFacility.Name = facility.Name;
            trackedFacility.Description = facility.Description;
            trackedFacility.StreetAddress = facility.StreetAddress;
            trackedFacility.Latitude = facility.Location?.Latitude;
            trackedFacility.Longitude = facility.Location?.Longitude;
            trackedFacility.Images = facility.Images.ToJson();
        }
    }
}
