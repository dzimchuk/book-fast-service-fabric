using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using BookFast.Web.Contracts.Models;

namespace BookFast.Web.Proxy
{
    internal class BookingProxy : IBookingService
    {
        private readonly IBookFastAPIFactory restClientFactory;
        private readonly IBookingMapper mapper;

        public BookingProxy(IBookFastAPIFactory restClientFactory, IBookingMapper mapper)
        {
            this.restClientFactory = restClientFactory;
            this.mapper = mapper;
        }

        public async Task BookAsync(Guid accommodationId, BookingDetails details)
        {
            var client = await restClientFactory.CreateAsync();

            var data = mapper.MapFrom(details);
            data.AccommodationId = accommodationId;

            var result = await client.CreateBookingWithHttpMessagesAsync(accommodationId, data);
            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new AccommodationNotFoundException(accommodationId);
        }

        public async Task<List<Booking>> ListPendingAsync()
        {
            var client = await restClientFactory.CreateAsync();
            var result = await client.ListBookingsWithHttpMessagesAsync();

            return mapper.MapFrom(result.Body);
        }

        public async Task CancelAsync(Guid id)
        {
            var client = await restClientFactory.CreateAsync();
            var result = await client.DeleteBookingWithHttpMessagesAsync(id);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new BookingNotFoundException(id);
        }

        public async Task<Booking> FindAsync(Guid id)
        {
            var client = await restClientFactory.CreateAsync();
            var result = await client.FindBookingWithHttpMessagesAsync(id);

            if (result.Response.StatusCode == HttpStatusCode.NotFound)
                throw new BookingNotFoundException(id);

            return mapper.MapFrom(result.Body);
        }
    }
}