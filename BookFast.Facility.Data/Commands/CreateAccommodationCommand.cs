using System.Threading.Tasks;
using BookFast.Facility.Data.Models;
using Accommodation = BookFast.Facility.Contracts.Models.Accommodation;
using BookFast.SeedWork;

namespace BookFast.Facility.Data.Commands
{
    internal class CreateAccommodationCommand : ICommand<FacilityContext>
    {
        private readonly Accommodation accommodation;
        private readonly IAccommodationMapper mapper;

        public CreateAccommodationCommand(Accommodation accommodation, IAccommodationMapper mapper)
        {
            this.accommodation = accommodation;
            this.mapper = mapper;
        }

        public Task ApplyAsync(FacilityContext model)
        {
            model.Accommodations.Add(mapper.MapFrom(accommodation));
            return model.SaveChangesAsync();
        }
    }
}