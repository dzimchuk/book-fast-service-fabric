using BookFast.SeedWork.Swagger;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookFast.Facility.CommandStack.Commands
{
    public class CreateAccommodationCommand : IRequest<int>
    {
        [SwaggerIgnore]
        public int FacilityId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Range(0, 20)]
        public int RoomCount { get; set; }

        public string[] Images { get; set; }
    }
}
