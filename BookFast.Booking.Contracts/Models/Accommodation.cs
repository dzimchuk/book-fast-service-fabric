using System;

namespace BookFast.Booking.Contracts.Models
{
    public class Accommodation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RoomCount { get; set; }
        public string[] Images { get; set; }

        public int FacilityId { get; set; }
    }
}
