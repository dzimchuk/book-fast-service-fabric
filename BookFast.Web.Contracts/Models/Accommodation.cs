using System;

namespace BookFast.Web.Contracts.Models
{
    public class Accommodation
    {
        public Guid Id { get; set; }
        public Guid FacilityId { get; set; }
        public AccommodationDetails Details { get; set; }
    }
}