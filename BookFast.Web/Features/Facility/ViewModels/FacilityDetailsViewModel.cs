using System.Collections.Generic;

namespace BookFast.Web.Features.Facility.ViewModels
{
    public class FacilityDetailsViewModel
    {
        public FacilityViewModel Facility { get; set; }
        public IEnumerable<AccommodationViewModel> Accommodations { get; set; } 
    }
}