using BookFast.SeedWork.Modeling;

namespace BookFast.Facility.Domain.Events
{
    public class FacilityUpdatedEvent : Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string StreetAddress { get; set; }

        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        public string[] Images { get; set; }
    }
}
