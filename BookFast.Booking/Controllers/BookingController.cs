using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using MediatR;
using BookFast.Booking.QueryStack;
using BookFast.Booking.QueryStack.Representations;
using BookFast.Booking.Domain.Exceptions;
using BookFast.Booking.CommandStack.Commands;
using BookFast.Security;

namespace BookFast.Booking.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IMediator mediator;
        private readonly IBookingQueryDataSource queryDataSource;
        private readonly TestOptions testOptions;
        private readonly ISecurityContext securityContext;

        public BookingController(IMediator mediator, IBookingQueryDataSource queryDataSource, IOptions<TestOptions> testOptions, ISecurityContext securityContext)
        {
            this.mediator = mediator;
            this.queryDataSource = queryDataSource;
            this.testOptions = testOptions.Value;
            this.securityContext = securityContext;
        }

        /// <summary>
        /// List bookings by customer
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/bookings")]
        [SwaggerOperation("list-bookings")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(IEnumerable<BookingRepresentation>))]
        public Task<IEnumerable<BookingRepresentation>> List()
        {
            FailRandom();

            return queryDataSource.ListPendingAsync(securityContext.GetCurrentUser());
        }

        /// <summary>
        /// Find booking by ID
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns></returns>
        [HttpGet("api/bookings/{id}")]
        [SwaggerOperation("find-booking")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(BookingRepresentation))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Booking not found")]
        public async Task<IActionResult> Find(Guid id)
        {
            try
            {
                var booking = await queryDataSource.FindAsync(id);
                if (booking == null)
                {
                    throw new BookingNotFoundException(id);
                }

                return Ok(booking);
            }
            catch (BookingNotFoundException ex)
            {
                return NotFound(ex);
            }
        }

        /// <summary>
        /// Book an accommodation
        /// </summary>
        /// <param name="accommodationId">Accommodation ID</param>
        /// <param name="bookingData">Booking details</param>
        /// <returns></returns>
        [HttpPost("api/accommodations/{accommodationId}/bookings")]
        [SwaggerOperation("create-booking")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Created)]
        [SwaggerResponse((int)System.Net.HttpStatusCode.BadRequest, Description = "Invalid parameters")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Accommodation not found")]
        public async Task<IActionResult> Create([FromRoute]int accommodationId, [FromBody]BookAccommodationCommand bookingData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bookingData.AccommodationId = accommodationId;
                    var bookingId = await mediator.Send(bookingData);

                    return CreatedAtAction("Find", new { id = bookingId }, null);
                }

                return BadRequest();
            }
            catch (AccommodationNotFoundException ex)
            {
                return NotFound(ex);
            }
        }

        private void FailRandom()
        {
            if (testOptions.FailRandom)
            {
                if (GenerateRandom() < 0)
                {
                    throw new Exception("test");
                }
            }
        }

        private static int GenerateRandom()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[5];
                rng.GetBytes(bytes);
                return BitConverter.ToInt32(bytes, 0);
            }
        }

        /// <summary>
        /// Cancel booking
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns></returns>
        [HttpDelete("api/bookings/{id}")]
        [SwaggerOperation("delete-booking")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent)]
        [SwaggerResponse((int)System.Net.HttpStatusCode.BadRequest, Description = "Attempt to cancel a booking of another user")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NotFound, Description = "Booking not found")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await mediator.Send(new CancelBookingCommand { Id = id });
                return new NoContentResult();
            }
            catch (BookingNotFoundException)
            {
                return NotFound();
            }
            catch (UserMismatchException)
            {
                return BadRequest();
            }
        }
    }
}