using System;

namespace BookFast.SeedWork.Validation
{
    public interface IDateRange
    {
        DateTimeOffset FromDate { get; set; }
        DateTimeOffset ToDate { get; set; }
    }
}
