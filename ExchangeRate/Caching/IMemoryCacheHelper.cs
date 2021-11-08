using System;
using System.Threading.Tasks;

namespace ExchangeRate.Caching
{
    public interface IMemoryCacheHelper
    {
        Task<T> GetOrAddAsync<T>(
            string key, Func<Task<T>> factory, TimeSpan delay);

        void Set<TItem>(string key, TItem item, TimeSpan delay);
        T Get<T>(string key);
        void Remove(string key);
        void RemoveAll();
    }
}
