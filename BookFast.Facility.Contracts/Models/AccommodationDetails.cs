namespace BookFast.Facility.Contracts.Models
{
    public class AccommodationDetails
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int RoomCount { get; set; }
        public string[] Images { get; set; }
    }
}