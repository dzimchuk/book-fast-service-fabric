using System;

namespace BookFast.Booking.QueryStack.Representations
{
    public class BookingRepresentation
    {
        /// <summary>
        /// Booking ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Accommodation ID
        /// </summary>
        public int AccommodationId { get; set; }

        /// <summary>
        /// Accommodation name
        /// </summary>
        public string AccommodationName { get; set; }

        /// <summary>
        /// Facility ID
        /// </summary>
        public int FacilityId { get; set; }

        /// <summary>
        /// Facility name
        /// </summary>
        public string FacilityName { get; set; }

        /// <summary>
        /// Facility street address
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Booking start date
        /// </summary>
        public DateTimeOffset FromDate { get; set; }

        /// <summary>
        /// Booking end date
        /// </summary>
        public DateTimeOffset ToDate { get; set; }
    }
}