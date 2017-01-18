using System;

namespace BookFast.Booking.Contracts.Models
{
    public class Facility
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StreetAddress { get; set; }
    }
}
