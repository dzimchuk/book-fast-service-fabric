using System.Collections.Generic;
using BookFast.Web.Contracts.Models;
using BookFast.Web.ViewModels;

namespace BookFast.Web.Controllers
{
    public interface IAccommodationMapper
    {
        AccommodationViewModel MapFrom(Accommodation accommodation);
        IEnumerable<AccommodationViewModel> MapFrom(IEnumerable<Accommodation> accommodations);
        AccommodationDetails MapFrom(AccommodationViewModel viewModel);
    }
}