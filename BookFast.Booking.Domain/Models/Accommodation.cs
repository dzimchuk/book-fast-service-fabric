using BookFast.SeedWork.Modeling;

namespace BookFast.Booking.Domain.Models
{
    public class Accommodation : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int RoomCount { get; set; }
        public string[] Images { get; set; }

        public int FacilityId { get; set; }
    }
}
