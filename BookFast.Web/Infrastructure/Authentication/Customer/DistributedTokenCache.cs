using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Identity.Client;
using System;

namespace BookFast.Web.Infrastructure.Authentication.Customer
{
    internal class DistributedTokenCache
    {
        private readonly IDistributedCache distributedCache;
        private readonly string userId;

        private readonly TokenCache tokenCache = new TokenCache();

        public DistributedTokenCache(IDistributedCache cache, string userId)
        {
            this.distributedCache = cache;
            this.userId = userId;

            tokenCache.SetBeforeAccess(OnBeforeAccess);
            tokenCache.SetAfterAccess(OnAfterAccess);
        }

        public TokenCache GetMSALCache() => tokenCache;

        private void OnBeforeAccess(TokenCacheNotificationArgs args)
        {
            var userTokenCachePayload = distributedCache.Get(CacheKey);
            if (userTokenCachePayload != null)
            {
                tokenCache.Deserialize(userTokenCachePayload);
            }
        }

        private void OnAfterAccess(TokenCacheNotificationArgs args)
        {
            if (tokenCache.HasStateChanged)
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(14)
                };

                distributedCache.Set(CacheKey, tokenCache.Serialize(), cacheOptions);

                tokenCache.HasStateChanged = false;
            }
        }

        private string CacheKey => $"TokenCache_{userId}";

    }
}