using System;
using ExchangeRate.Entities;
using FluentValidation;
using System.Text.RegularExpressions;
using ExchangeRate.Extensions;

namespace ExchangeRate.Validations
{
    public class XERequestValidator: AbstractValidator<XERatesRequest>
    {
        public XERequestValidator()
        {
            RuleFor(x => x.Amount)
                .Must(field => field >= 0)
                .WithMessage(ErrorMessages.AmountInvalid.GetDescription());

            RuleFor(x => x.margin)
                .Must(field => field >= 0)
                .WithMessage(ErrorMessages.MarginInvalid.GetDescription());

            RuleFor(x => x.toCurrency)
                .Must(field => !String.IsNullOrEmpty(field))
                .WithMessage(ErrorMessages.ConvertToCurrencyInvalid.GetDescription());
        }
    }
}
