using BookFast.SeedWork.Modeling;

namespace BookFast.Facility.Domain.Events
{
    public class FacilityCreatedEvent : Event
    {
        private readonly Models.Facility facility;
        private int? id;

        public FacilityCreatedEvent()
        {
        }

        public FacilityCreatedEvent(Models.Facility facility)
        {
            this.facility = facility;

            Name = facility.Name;
            Description = facility.Description;
            Owner = facility.Owner;
            StreetAddress = facility.StreetAddress;
            Longitude = facility.Location?.Longitude;
            Latitude = facility.Location?.Latitude;
            Images = facility.Images;
        }

        public int Id
        {
            get => id ?? facility.Id;
            set => id = value;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string StreetAddress { get; set; }

        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        public string[] Images { get; set; }
    }
}
