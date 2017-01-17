using System;
using System.ComponentModel.DataAnnotations;
using BookFast.Booking.Validation;

namespace BookFast.Booking.Models
{
    [DateRange(ErrorMessage = "End date should be greater than or equal to start date")]
    public class BookingData
    {
        /// <summary>
        /// Accommodation ID
        /// </summary>
        public Guid AccommodationId { get; set; }

        /// <summary>
        /// Booking start date
        /// </summary>
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Start date cannot be in the past")]
        public DateTimeOffset FromDate { get; set; }

        /// <summary>
        /// Booking end date
        /// </summary>
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "End date cannot be in the past")]
        public DateTimeOffset ToDate { get; set; }
    }
}