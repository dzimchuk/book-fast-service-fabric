using System;
using System.ComponentModel.DataAnnotations;

namespace BookFast.Web.Features.Facility.ViewModels
{
    public class AccommodationViewModel
    {
        public Guid Id { get; set; }
        public Guid FacilityId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Range(0, 20)]
        [Display(Name = "Number of rooms")]
        public int RoomCount { get; set; }

        public string[] Images { get; set; }
    }
}