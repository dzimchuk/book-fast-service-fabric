using System;
using System.ComponentModel.DataAnnotations;

namespace BookFast.Facility.QueryStack.Representations
{
    public class FacilityRepresentation
    {
        /// <summary>
        /// Facility ID
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Facility name
        /// </summary>
        [Required]
        public string Name { get; set; }
        
        /// <summary>
        /// Facility description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Facility street address
        /// </summary>
        [Required]
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
        [Required]
        public int AccommodationCount { get; set; }
    }
}