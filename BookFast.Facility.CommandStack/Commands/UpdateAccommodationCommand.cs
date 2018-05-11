using BookFast.SeedWork.Swagger;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookFast.Facility.CommandStack.Commands
{
    public class UpdateAccommodationCommand : IRequest
    {
        [SwaggerIgnore]
        public int AccommodationId { get; set; }

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
