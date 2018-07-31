using BookFast.SeedWork.Swagger;
using BookFast.SeedWork.Validation;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace BookFast.Booking.CommandStack.Commands
{
    [DateRange(ErrorMessage = "End date should be greater than or equal to start date")]
    public class BookAccommodationCommand : IRequest<Guid>, IDateRange
    {
        [SwaggerIgnore]
        public int AccommodationId { get; set; }

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
