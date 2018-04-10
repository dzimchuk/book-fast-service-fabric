using System;

namespace BookFast.Web.Contracts.Models
{
    public class Facility
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public FacilityDetails Details { get; set; }
        public Location Location { get; set; }
        public int AccommodationCount { get; set; }
    }
}