using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRate.Entities;

namespace ExchangeRate.Services.Interfaces
{
    public interface IXEService
    {
        Task<XERatesResponse> GetExchangeRates(XERatesRequest request);
        Task<List<Currency>> GetCurrencies();
        Task<List<Currency>> GetCachedCurrencies();
    }
}
