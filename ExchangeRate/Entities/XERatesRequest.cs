using System;
namespace ExchangeRate.Entities
{
    public class XERatesRequest
    {
        public string baseCurrency { get; set; }
        public string toCurrency { get; set; }
        public double Amount { get; set; }
        public double margin { get; set; }
    }
}
