using System;
namespace ExchangeRate.Exceptions
{
    public class GenericBusinessException : Exception
    {
        public GenericBusinessException()
        {
        }

        public GenericBusinessException(string message) : base(message)
        {
        }
    }
}
