using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRate.Entities;
using ExchangeRate.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExchangeRateApi.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/api")]
    [ApiVersion("1.0")]
    public class ExchangeRateController : ControllerBase
    {

        private readonly ILogger<ExchangeRateController> _logger;
        private readonly IXEService _xeService;

        public ExchangeRateController(ILogger<ExchangeRateController> logger, IXEService xEService)
        {
            _logger = logger;
            _xeService = xEService;
        }

        [HttpGet]
        [Route("rates")]
        //[Authorize]
        public async Task<XERatesResponse> Get(string _ToCurrency, int _Amount=0, double _Margin=0, string _BaseCurrency="")
        {
            XERatesRequest request = new XERatesRequest
            {
                baseCurrency = _BaseCurrency,
                toCurrency = _ToCurrency,
                Amount = _Amount,
                margin = _Margin
            };
            return await _xeService.GetExchangeRates(request);
        }

        [HttpGet]
        [Route("currencies")]
        public async Task<List<Currency>> Get()
        {
            return await _xeService.GetCachedCurrencies();
        }
    }
}
