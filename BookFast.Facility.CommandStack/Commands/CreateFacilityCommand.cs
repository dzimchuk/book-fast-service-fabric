using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookFast.Facility.CommandStack.Commands
{
    public class CreateFacilityCommand : IRequest<int>
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string StreetAddress { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string[] Images { get; set; }
    }
}
