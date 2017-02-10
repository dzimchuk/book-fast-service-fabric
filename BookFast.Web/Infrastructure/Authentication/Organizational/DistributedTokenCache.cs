using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace BookFast.Web.Infrastructure.Authentication.Organizational
{
    internal class DistributedTokenCache : TokenCache
    {
        private readonly IDistributedCache cache;
        private readonly string userId;

        public DistributedTokenCache(IDistributedCache cache, string userId)
        {
            this.cache = cache;
            this.userId = userId;

            BeforeAccess = OnBeforeAccess;
            AfterAccess = OnAfterAccess;
        }

        private void OnBeforeAccess(TokenCacheNotificationArgs args)
        {
            var userTokenCachePayload = cache.Get(CacheKey);
            if (userTokenCachePayload != null)
            {
                Deserialize(userTokenCachePayload);
            }
        }

        private void OnAfterAccess(TokenCacheNotificationArgs args)
        {
            if (HasStateChanged)
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(14)
                };

                cache.Set(CacheKey, Serialize(), cacheOptions);

                HasStateChanged = false;
            }
        }

        private string CacheKey => $"TokenCache_{userId}";
    }
}
