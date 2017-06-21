using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using BookFast.Booking.Contracts;
using BookFast.Booking.Models.Representations;
using BookFast.Booking.Contracts.Exceptions;
using BookFast.Booking.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace BookFast.Booking.Controllers
{
    [Authorize]
    [SwaggerResponseRemoveDefaults]
    public class BookingController : Controller
    {
        private readonly IBookingService service;
        private readonly IBookingMapper mapper;
        private readonly IConfiguration configuration;

        public BookingController(IBookingService service, IBookingMapper mapper, IConfiguration configuration)
        {
            this.service = service;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        /// <summary>
        /// List bookings by customer
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/bookings")]
        [SwaggerOperation("list-bookings")]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(IEnumerable<BookingRepresentation>))]
        public async Task<IEnumerable<BookingRepresentation>> List()
        {
            FailRandom();

            var bookings = await service.ListPendingAsync();
            return mapper.MapFrom(bookings);
        }

        /// <summary>
        /// Find booking by ID
        /// </summary>
        /// <param name="id">Booking ID</param>
        /// <returns></returns>
        [HttpGet("api/bookings/{id}")]
        [SwaggerOperation("find-booking")]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(BookingRepresentation))]
        [SwaggerResponse(System.Net.HttpStatusCode.NotFound, Description = "Booking not found")]
        public async Task<IActionResult> Find(Guid id)
        {
            try
            {
                var booking = await service.FindAsync(id);
                return Ok(mapper.MapFrom(booking));
            }
            catch (BookingNotFoundException)
            {
                return NotFound();
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
        [SwaggerResponse(System.Net.HttpStatusCode.Created, Type = typeof(BookingRepresentation))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Description = "Invalid parameters")]
        [SwaggerResponse(System.Net.HttpStatusCode.NotFound, Description = "Accommodation not found")]
        public async Task<IActionResult> Create([FromRoute]Guid accommodationId, [FromBody]BookingData bookingData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var booking = await service.BookAsync(accommodationId, mapper.MapFrom(bookingData));
                    return CreatedAtAction("Find", new { id = booking.Id }, mapper.MapFrom(booking));
                }

                return BadRequest();
            }
            catch (AccommodationNotFoundException)
            {
                return NotFound();
            }
        }

        private void FailRandom()
        {
            if (bool.Parse(configuration["Test:FailRandom"]))
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
        [SwaggerResponse(System.Net.HttpStatusCode.NoContent)]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Description = "Attempt to cancel a booking of another user")]
        [SwaggerResponse(System.Net.HttpStatusCode.NotFound, Description = "Booking not found")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await service.CancelAsync(id);
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