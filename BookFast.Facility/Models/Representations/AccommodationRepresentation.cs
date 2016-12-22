using System;

namespace BookFast.Facility.Models.Representations
{
    public class AccommodationRepresentation
    {
        /// <summary>
        /// Accommodation ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Facility ID
        /// </summary>
        public Guid FacilityId { get; set; }

        /// <summary>
        /// Accommodation name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Accommodation description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Number of rooms
        /// </summary>
        public int RoomCount { get; set; }

        /// <summary>
        /// Accommodation images
        /// </summary>
        public string[] Images { get; set; }
    }
}