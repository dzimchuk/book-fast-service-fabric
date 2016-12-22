using System.ComponentModel.DataAnnotations;

namespace BookFast.Facility.Models
{
    public class FacilityData
    {
        /// <summary>
        /// Facility name
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Facility description
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Facility street address
        /// </summary>
        [Required]
        [StringLength(100)]
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
    }
}