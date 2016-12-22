using System;

namespace BookFast.Facility.Models.Representations
{
    public class FacilityRepresentation
    {
        /// <summary>
        /// Facility ID
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Facility name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Facility description
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Facility street address
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Facility images
        /// </summary>
        public string[] Images { get; set; }

        /// <summary>
        /// Number of accommodations
        /// </summary>
        public int AccommodationCount { get; set; }
    }
}