using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ExchangeRate.Entities;
using ExchangeRate.Services.Interfaces;
using ExchangeRate.HttpClients;
using System.Collections.Generic;
using ExchangeRate.Caching;
using FluentValidation;
using ExchangeRate.Exceptions;
using ExchangeRate.Validations;

namespace ExchangeRate.Services
{
    public class XEService : IXEService
    {
        private readonly ILogger<XEService> _logger;
        private readonly IXEClient _xeClient;
        private readonly IMemoryCacheHelper _memoryCacheHelper;
        private readonly TimeSpan _timeSpan;
        private const string CurrenciesCacheKey = "AllCurrencies";

        public XEService(ILogger<XEService> logger, IXEClient xeClient, IMemoryCacheHelper memoryCacheHelper)
        {
            _logger = logger;
            _xeClient = xeClient;
            _memoryCacheHelper = memoryCacheHelper;
            _timeSpan = TimeSpan.FromHours(2);
        }

        public async Task<XERatesResponse> GetExchangeRates(XERatesRequest request)
        {
            try
            {
                var validator = new XERequestValidator();
                validator.ValidateAndThrow(request);
                List<Currency> currencies = await GetCachedCurrencies();
                
                return await _xeClient.GetExchangeRatesAsync(request);
            }
            catch (ValidationException ex)
            {
                throw new GenericBusinessException(ex.Message);
            }
            
        }

        public async Task<List<Currency>> GetCachedCurrencies()
        {
            var retVal = await _memoryCacheHelper.GetOrAddAsync(CurrenciesCacheKey, () => Task.Run(GetCurrencies), _timeSpan);
            return retVal;
        }

        public async Task<List<Currency>> GetCurrencies()
        {
            var retVal = await _xeClient.GetCurrenciesAsync();
            return retVal;
        }
    }
}
