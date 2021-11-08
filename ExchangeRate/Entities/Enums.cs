using System;
using System.ComponentModel;

namespace ExchangeRate.Entities
{
    public enum ErrorMessages
    {
        [Description("Amount empty or invalid")]
        AmountInvalid,

        [Description("Margin empty or invalid")]
        MarginInvalid,

        [Description("Convert to currency empty or invalid")]
        ConvertToCurrencyInvalid
    }
}
