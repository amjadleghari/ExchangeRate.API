using System;
using System.Collections.Generic;

namespace ExchangeRate.Entities
{
    public class XECurrenciesResponse
    {
        public string terms { get; set; }
        public string privacy { get; set; }
        public List<Currency> currencies { get; set; }
    }
}
