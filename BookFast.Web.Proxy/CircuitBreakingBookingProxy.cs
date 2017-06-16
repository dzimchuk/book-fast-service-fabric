using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Models;
using Polly;
using Polly.CircuitBreaker;

namespace BookFast.Web.Proxy
{
    internal class CircuitBreakingBookingProxy : IBookingService
    {
        private readonly IBookingService innerProxy;
        private readonly CircuitBreakerPolicy breaker =
            Policy.Handle<Exception>().CircuitBreaker(
                exceptionsAllowedBeforeBreaking: 2,
                durationOfBreak: TimeSpan.FromMinutes(1));

        public CircuitBreakingBookingProxy(IBookingService innerProxy)
        {
            this.innerProxy = innerProxy;
        }

        public Task BookAsync(Guid facilityId, Guid accommodationId, BookingDetails details)
        {
            return breaker.ExecuteAsync(() => innerProxy.BookAsync(facilityId, accommodationId, details));
        }

        public Task CancelAsync(Guid facilityId, Guid id)
        {
            return breaker.ExecuteAsync(() => innerProxy.CancelAsync(facilityId, id));
        }

        public Task<Contracts.Models.Booking> FindAsync(Guid facilityId, Guid id)
        {
            return breaker.ExecuteAsync(() => innerProxy.FindAsync(facilityId, id));
        }

        public Task<List<Contracts.Models.Booking>> ListPendingAsync()
        {
            return breaker.ExecuteAsync(() => innerProxy.ListPendingAsync());
        }
    }
}
