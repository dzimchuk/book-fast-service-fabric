using System.Threading.Tasks;
using BookFast.Facility.Data.Models;
using BookFast.SeedWork;

namespace BookFast.Facility.Data.Commands
{
    internal class CreateFacilityCommand : ICommand<FacilityContext>
    {
        private readonly Contracts.Models.Facility facility;
        private readonly IFacilityMapper mapper;

        public CreateFacilityCommand(Contracts.Models.Facility facility, IFacilityMapper mapper)
        {
            this.facility = facility;
            this.mapper = mapper;
        }

        public Task ApplyAsync(FacilityContext model)
        {
            model.Facilities.Add(mapper.MapFrom(facility));
            return model.SaveChangesAsync();
        }
    }
}