using System;

namespace BookFast.Web.Contracts.Models
{
    public class Accommodation
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public AccommodationDetails Details { get; set; }
    }
}