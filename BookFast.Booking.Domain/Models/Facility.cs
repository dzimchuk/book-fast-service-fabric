using BookFast.SeedWork.Modeling;

namespace BookFast.Booking.Domain.Models
{
    public class Facility : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string StreetAddress { get; set; }
    }
}
