using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRate.Entities;

namespace ExchangeRate.HttpClients
{
    public interface IXEClient
    {
        Task<XERatesResponse> GetExchangeRatesAsync(XERatesRequest currencyRequest);
        Task<List<Currency>> GetCurrenciesAsync();
    }
}
