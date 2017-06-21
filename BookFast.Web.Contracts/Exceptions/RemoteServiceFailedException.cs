using System;

namespace BookFast.Web.Contracts.Exceptions
{
    public class RemoteServiceFailedException : Exception
    {
        public RemoteServiceFailedException(int statusCode, Exception innerException)
            : base(innerException.Message, innerException)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }
    }
}
