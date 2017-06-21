using Microsoft.Rest;
using Polly.CircuitBreaker;

namespace BookFast.Web.Proxy
{
    internal static class Extensions
    {
        public static int StatusCode(this HttpOperationException exception)
        {
            return (int)exception.Response.StatusCode;
        }

        public static int StatusCode(this BrokenCircuitException exception)
        {
            return StatusCode((HttpOperationException)exception.InnerException);
        }
    }
}
