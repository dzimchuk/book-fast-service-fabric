using System;
using System.Collections.Generic;

namespace BookFast.Facility.Data.Models
{
    public class Facility
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string StreetAddress { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int AccommodationCount { get; set; }
        public string Images { get; set; }
        public ICollection<Accommodation> Accommodations { get; set; }
    }
}