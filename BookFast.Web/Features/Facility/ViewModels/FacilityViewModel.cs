using System.ComponentModel.DataAnnotations;

namespace BookFast.Web.Features.Facility.ViewModels
{
    public class FacilityViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Street address")]
        public string StreetAddress { get; set; }

        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        public string[] Images { get; set; }

        public int AccommodationCount { get; set; }
    }
}