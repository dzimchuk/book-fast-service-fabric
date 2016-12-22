using System;

namespace BookFast.Facility.Contracts.Models
{
    public class Facility
    {
        public Guid Id { get; set; }
        public string Owner { get; set; }
        public FacilityDetails Details { get; set; }
        public Location Location { get; set; }
        public int AccommodationCount { get; set; }
    }
}