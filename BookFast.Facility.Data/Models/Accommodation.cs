using System;
using System.Collections.Generic;

namespace BookFast.Facility.Data.Models
{
    public class Accommodation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid FacilityId { get; set; }
        public int RoomCount { get; set; }
        public Facility Facility { get; set; }
        public string Images { get; set; }
    }
}