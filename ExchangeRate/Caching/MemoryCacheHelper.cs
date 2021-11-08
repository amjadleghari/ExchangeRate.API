using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRate.Caching
{
    /// <summary>
    /// .SetSize(1), Size amount
    /// .SetPriority, Priority on removing when reaching size limit
    /// .SetSlidingExpiration, Keep in cache for this time, reset it if accessed.
    /// .SetAbsoluteExpiration Expired from cache after this time
    /// </summary>
    public class MemoryCacheHelper : IMemoryCacheHelper
    {
        private readonly MemoryCache _cache;

        public MemoryCacheHelper(MemoryCache cache)
        {
            _cache = cache;
        }

        public Task<T> GetOrAddAsync<T>(
            string key, Func<Task<T>> factory, TimeSpan delay)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1)
                .SetPriority(CacheItemPriority.High)
                .SetAbsoluteExpiration(delay);

            return _cache.GetOrCreateAsync(key, async cacheEntry =>
            {
                cacheEntry.SetOptions(cacheEntryOptions);
                var value = await factory().ConfigureAwait(false);
                return value;
            });
        }

        public void Set<TItem>(string key, TItem item, TimeSpan delay)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1)
                .SetPriority(CacheItemPriority.High)
                .SetAbsoluteExpiration(delay);
            _cache.Set(key, item, cacheEntryOptions);
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void RemoveAll()
        {
            _cache.Compact(1);
        }
    }

}
