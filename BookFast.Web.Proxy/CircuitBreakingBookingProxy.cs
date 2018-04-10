using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookFast.Web.Contracts;
using BookFast.Web.Contracts.Exceptions;
using BookFast.Web.Contracts.Models;
using Microsoft.Rest;
using Polly;
using Polly.CircuitBreaker;

namespace BookFast.Web.Proxy
{
    internal class CircuitBreakingBookingProxy : IBookingProxy
    {
        private readonly IBookingProxy innerProxy;
        private readonly CircuitBreakerPolicy breaker =
            Policy.Handle<HttpOperationException>(ex => ex.StatusCode() >= 500 || ex.StatusCode() == 429)
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 2,
                durationOfBreak: TimeSpan.FromMinutes(1));

        public CircuitBreakingBookingProxy(IBookingProxy innerProxy)
        {
            this.innerProxy = innerProxy;
        }

        public async Task BookAsync(int facilityId, int accommodationId, BookingDetails details)
        {
            try
            {
                await breaker.ExecuteAsync(() => innerProxy.BookAsync(facilityId, accommodationId, details));
            }
            catch (HttpOperationException ex)
            {
                throw new RemoteServiceFailedException(ex.StatusCode(), ex);
            }
            catch (BrokenCircuitException ex)
            {
                throw new RemoteServiceFailedException(ex.StatusCode(), ex);
            }
        }

        public async Task CancelAsync(int facilityId, Guid id)
        {
            try
            {
                await breaker.ExecuteAsync(() => innerProxy.CancelAsync(facilityId, id));
            }
            catch (HttpOperationException ex)
            {
                throw new RemoteServiceFailedException(ex.StatusCode(), ex);
            }
            catch (BrokenCircuitException ex)
            {
                throw new RemoteServiceFailedException(ex.StatusCode(), ex);
            }
        }

        public async Task<Contracts.Models.Booking> FindAsync(int facilityId, Guid id)
        {
            try
            {
                return await breaker.ExecuteAsync(() => innerProxy.FindAsync(facilityId, id));
            }
            catch (HttpOperationException ex)
            {
                throw new RemoteServiceFailedException(ex.StatusCode(), ex);
            }
            catch (BrokenCircuitException ex)
            {
                throw new RemoteServiceFailedException(ex.StatusCode(), ex);
            }
        }

        public async Task<List<Contracts.Models.Booking>> ListPendingAsync()
        {
            try
            {
                return await breaker.ExecuteAsync(() => innerProxy.ListPendingAsync());
            }
            catch (HttpOperationException ex)
            {
                throw new RemoteServiceFailedException(ex.StatusCode(), ex);
            }
            catch (BrokenCircuitException ex)
            {
                throw new RemoteServiceFailedException(ex.StatusCode(), ex);
            }
        }
    }
}
