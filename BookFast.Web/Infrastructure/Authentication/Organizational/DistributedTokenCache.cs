using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;

namespace BookFast.Web.Infrastructure.Authentication.Organizational
{
    internal class DistributedTokenCache : TokenCache
    {
        private readonly IDistributedCache cache;
        private readonly string userId;

        private UserTokenCacheItem currentCacheItem;

        public DistributedTokenCache(IDistributedCache cache, string userId)
        {
            this.cache = cache;
            this.userId = userId;

            BeforeAccess = OnBeforeAccess;
            AfterAccess = OnAfterAccess;
        }

        private void OnBeforeAccess(TokenCacheNotificationArgs args)
        {
            var serializedCacheItem = cache.GetString(CacheKey);
            if (serializedCacheItem == null)
            {
                return;
            }

            var cacheItem = JsonConvert.DeserializeObject<UserTokenCacheItem>(serializedCacheItem);

            if (currentCacheItem == null || currentCacheItem.LastWrite < cacheItem.LastWrite)
            {
                currentCacheItem = cacheItem;
                Deserialize(currentCacheItem.Payload);
            }
        }

        private void OnAfterAccess(TokenCacheNotificationArgs args)
        {
            if (HasStateChanged)
            {
                var cacheItem = new UserTokenCacheItem
                {
                    Payload = Serialize(),
                    LastWrite = DateTimeOffset.Now
                };

                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(14)
                };
                cache.SetString(CacheKey, JsonConvert.SerializeObject(cacheItem), cacheOptions);

                HasStateChanged = false;
            }
        }

        private string CacheKey => $"TokenCache_{userId}";
    }
}
