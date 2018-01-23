using System.Threading.Tasks;
using BookFast.Facility.Data.Models;
using BookFast.SeedWork;

namespace BookFast.Facility.Data.Commands
{
    internal class CreateFacilityCommand : ICommand<BookFastContext>
    {
        private readonly Contracts.Models.Facility facility;
        private readonly IFacilityMapper mapper;

        public CreateFacilityCommand(Contracts.Models.Facility facility, IFacilityMapper mapper)
        {
            this.facility = facility;
            this.mapper = mapper;
        }

        public Task ApplyAsync(BookFastContext model)
        {
            model.Facilities.Add(mapper.MapFrom(facility));
            return model.SaveChangesAsync();
        }
    }
}