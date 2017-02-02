using System;

namespace BookFast.Web.Infrastructure.Authentication
{
    internal class UserTokenCacheItem
    {
        public byte[] Payload { get; set; }
        public DateTimeOffset LastWrite { get; set; }
    }
}
