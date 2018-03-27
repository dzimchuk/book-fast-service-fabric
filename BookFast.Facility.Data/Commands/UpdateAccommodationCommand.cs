using System.Threading.Tasks;
using BookFast.Facility.Data.Models;
using Accommodation = BookFast.Facility.Contracts.Models.Accommodation;
using Microsoft.EntityFrameworkCore;
using BookFast.SeedWork;

namespace BookFast.Facility.Data.Commands
{
    internal class UpdateAccommodationCommand : ICommand<FacilityContext>
    {
        private readonly Accommodation accommodation;
        private readonly IAccommodationMapper mapper;

        public UpdateAccommodationCommand(Accommodation accommodation, IAccommodationMapper mapper)
        {
            this.accommodation = accommodation;
            this.mapper = mapper;
        }

        public async Task ApplyAsync(FacilityContext model)
        {
            var existingAccommodation = await model.Accommodations.FirstOrDefaultAsync(a => a.Id == accommodation.Id);
            if (existingAccommodation != null)
            {
                var dm = mapper.MapFrom(accommodation);

                existingAccommodation.Name = dm.Name;
                existingAccommodation.Description = dm.Description;
                existingAccommodation.RoomCount = dm.RoomCount;
                existingAccommodation.Images = dm.Images;

                await model.SaveChangesAsync();
            }
        }
    }
}