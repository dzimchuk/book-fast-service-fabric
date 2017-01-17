using System;

namespace BookFast.Search.Contracts.Models
{
    public class Accommodation
    {
        public Guid Id { get; set; }
        public Guid FacilityId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string FacilityName { get; set; }
        public string FacilityDescription { get; set; }

        public Location FacilityLocation { get; set; }

        public int RoomCount { get; set; }
        public string[] Images { get; set; }
    }
}
