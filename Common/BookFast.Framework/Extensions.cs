using System;
using System.Linq;

namespace BookFast.Framework
{
    public static class Extensions
    {
        public static long ToPartitionKey(this Guid id)
        {
            var first = id.ToString().ToUpperInvariant().First();
            var offset = first - '0';
            if (offset <= 9)
            {
                return offset;
            }

            return first - 'A' + 10;
        }
    }
}
