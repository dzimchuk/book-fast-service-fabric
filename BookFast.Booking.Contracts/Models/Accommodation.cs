using System;

namespace BookFast.Booking.Contracts.Models
{
    public class Accommodation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RoomCount { get; set; }
        public string[] Images { get; set; }

        public Guid FacilityId { get; set; }
    }
}
