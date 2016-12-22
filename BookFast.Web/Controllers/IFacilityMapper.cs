using System.Collections.Generic;
using BookFast.Web.Contracts.Models;
using BookFast.Web.ViewModels;

namespace BookFast.Web.Controllers
{
    public interface IFacilityMapper
    {
        FacilityViewModel MapFrom(Facility facility);
        IEnumerable<FacilityViewModel> MapFrom(IEnumerable<Facility> facilities);
        FacilityDetails MapFrom(FacilityViewModel viewModel);
    }
}