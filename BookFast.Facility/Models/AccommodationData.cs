using System;
using System.ComponentModel.DataAnnotations;

namespace BookFast.Facility.Models
{
    public class AccommodationData
    {
        /// <summary>
        /// Accommodation name
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Accommodation description
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Number of rooms
        /// </summary>
        [Range(0, 20)]
        public int RoomCount { get; set; }

        /// <summary>
        /// Accommodation images
        /// </summary>
        public string[] Images { get; set; }
    }
}