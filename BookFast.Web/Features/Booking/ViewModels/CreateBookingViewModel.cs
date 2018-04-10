using System;
using System.ComponentModel.DataAnnotations;
using BookFast.SeedWork.Validation;

namespace BookFast.Web.Features.Booking.ViewModels
{
    [DateRange(ErrorMessage = "End date should be greater than or equal to start date")]
    public class CreateBookingViewModel : IDateRange
    {
        public int FacilityId { get; set; }
        public int AccommodationId { get; set; }

        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Start date cannot be in the past")]
        [Display(Name = "Start date")]
        public DateTimeOffset FromDate { get; set; }

        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "End date cannot be in the past")]
        [Display(Name = "End date")]
        public DateTimeOffset ToDate { get; set; }
    }
}