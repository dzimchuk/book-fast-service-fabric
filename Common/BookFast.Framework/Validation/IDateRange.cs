using System;

namespace BookFast.Framework.Validation
{
    public interface IDateRange
    {
        DateTimeOffset FromDate { get; set; }
        DateTimeOffset ToDate { get; set; }
    }
}
