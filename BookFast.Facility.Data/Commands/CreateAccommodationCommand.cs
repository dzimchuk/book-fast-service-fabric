using System.Threading.Tasks;
using BookFast.Facility.Data.Models;
using Accommodation = BookFast.Facility.Contracts.Models.Accommodation;
using BookFast.Framework;

namespace BookFast.Facility.Data.Commands
{
    internal class CreateAccommodationCommand : ICommand<BookFastContext>
    {
        private readonly Accommodation accommodation;
        private readonly IAccommodationMapper mapper;

        public CreateAccommodationCommand(Accommodation accommodation, IAccommodationMapper mapper)
        {
            this.accommodation = accommodation;
            this.mapper = mapper;
        }

        public Task ApplyAsync(BookFastContext model)
        {
            model.Accommodations.Add(mapper.MapFrom(accommodation));
            return model.SaveChangesAsync();
        }
    }
}