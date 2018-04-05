using System;

namespace BookFast.SeedWork
{
    public class FormattedException : Exception
    {
        protected FormattedException(string errorCode)
            : this(errorCode, null, null)
        {
        }

        protected FormattedException(string errorCode, string errorDescription)
            : this(errorCode, errorDescription, null)
        {
        }

        protected FormattedException(string errorCode, string errorDescription, Exception innerException)
            : base(FormatMessage(errorCode, errorDescription), innerException)
        {
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
        }

        public string ErrorCode { get; }
        public string ErrorDescription { get; }

        private static string FormatMessage(string errorCode, string errorDescription)
        {
            return !string.IsNullOrEmpty(errorDescription)
                ? $"{errorCode}: {errorDescription}"
                : errorCode;
        }
    }
}
