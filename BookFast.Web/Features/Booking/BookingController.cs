using System;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookFast.Web.Features.Booking.ViewModels;
using Microsoft.AspNetCore.Http;
using BookFast.Web.Infrastructure.Authentication.Customer;

namespace BookFast.Web.Features.Booking
{
    [Authorize(Policy = "Customer")]
    public class BookingController : Controller
    {
        private readonly IBookingProxy bookingService;
        private readonly BookingMapper mapper;
        private readonly IAccommodationProxy accommodationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public BookingController(IBookingProxy bookingService, BookingMapper mapper, IAccommodationProxy accommodationService, IHttpContextAccessor httpContextAccessor)
        {
            this.bookingService = bookingService;
            this.mapper = mapper;
            this.accommodationService = accommodationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        private string GetUserId()
        {
            return httpContextAccessor.HttpContext.User.FindFirst(B2CAuthConstants.ObjectIdClaimType).Value;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await bookingService.ListPendingAsync(GetUserId());
            return View(mapper.MapFrom(bookings));
        }

        public async Task<IActionResult> Create(int id)
        {
            try
            {
                var accommodation = await accommodationService.FindAsync(id);

                ViewBag.FacilityId = accommodation.FacilityId;
                ViewBag.AccommodationId = accommodation.Id;
                ViewBag.AccommodationName = accommodation.Details.Name;
                return View();
            }
            catch (AccommodationNotFoundException)
            {
                return View("NotFound");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookingViewModel booking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var details = mapper.MapFrom(booking);
                    await bookingService.BookAsync(GetUserId(), booking.AccommodationId, details);
                    return RedirectToAction("Index");
                }

                var accommodation = await accommodationService.FindAsync(booking.AccommodationId);
                ViewBag.AccommodationId = accommodation.Id;
                ViewBag.AccommodationName = accommodation.Details.Name;
                return View(booking);
            }
            catch (AccommodationNotFoundException)
            {
                return View("NotFound");
            }
        }

        public async Task<IActionResult> Cancel(Guid id, int facilityId)
        {
            try
            {
                var booking = await bookingService.FindAsync(GetUserId(), id);
                ViewBag.FacilityId = facilityId;
                return View(mapper.MapFrom(booking));
            }
            catch (BookingNotFoundException)
            {
                return View("NotFound");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Cancel")]
        public async Task<IActionResult> CancelConfirmed(Guid id, int facilityId)
        {
            try
            {
                await bookingService.CancelAsync(GetUserId(), id);
                return RedirectToAction("Index");
            }
            catch (BookingNotFoundException)
            {
                return View("NotFound");
            }
            catch (UserMismatchException)
            {
                return BadRequest();
            }
        }
    }
}