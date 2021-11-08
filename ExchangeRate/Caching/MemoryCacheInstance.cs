using System;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRate.Caching
{
    public static class MemoryCacheInstance
    {
        public static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions()
        {
            SizeLimit = 1024
        });
    }
}
