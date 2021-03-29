using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pelsoft.auth.common.storage
{
    public interface IStore
    {
        RefreshToken Get(string cacheKey);
        void Remove(string cacheKey);
        void Set(string cacheKey, RefreshToken token);
    }

    public class Store : IStore
    {

        private IMemoryCache _cache;

        public Store(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            //_cache.GetOrCreateAsync("Refresh", entry =>
            //{
            //    entry.SlidingExpiration = TimeSpan.FromSeconds(3);
            //    return Task.FromResult(DateTime.Now);
            //});
        }

        public void Set(string cacheKey, RefreshToken token)
        {
            MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
            cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddDays(2);
            //property specifies which objects should be removed as part of a strategy of the runtime to 
            //reclaim memory whenever the web server runs out of memory space.
            cacheExpirationOptions.Priority = CacheItemPriority.Normal;

            _cache.Set(cacheKey, token, cacheExpirationOptions);
        }

        public RefreshToken Get(string cacheKey)
        {
            var cacheEntry = _cache.Get<RefreshToken?>(cacheKey);
            return cacheEntry;
        }


        public void Remove(string cacheKey)
        {
            _cache.Remove(cacheKey);

        }
    }
}
