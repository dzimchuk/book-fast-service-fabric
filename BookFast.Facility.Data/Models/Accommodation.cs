namespace BookFast.Facility.Data.Models
{
    public class Accommodation
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int RoomCount { get; set; }
        public string Images { get; set; }

        public Facility Facility { get; set; }
    }
}