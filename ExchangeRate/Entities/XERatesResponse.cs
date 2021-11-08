using System;
using System.Collections.Generic;

namespace ExchangeRate.Entities
{
    public class XERatesResponse
    {
        public string terms { get; set; }
        public string privacy { get; set; }
        public string @from { get; set; }
        public double amount { get; set; }
        public string timestamp { get; set; }
        //[JsonProperty("to")]
        public List<ToCurrency> to { get; set; }

    }

    public class ToCurrency
    {
        //[JsonProperty("quotecurrency")]
        public string quotecurrency { get; set; }
        //[JsonProperty("mid")]
        public string mid { get; set; }
    }
}
