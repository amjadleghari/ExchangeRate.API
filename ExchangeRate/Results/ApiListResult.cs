using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRate.Results
{
    public class ApiListResult<T> : ApiResult
    {
        public ApiListResult(T content, bool status, IEnumerable<ErrorMessage> errors)
        {
            Data = content;
            if (errors != null && errors.Count() > 0)
            {
                Errors = new List<ErrorMessage>();
                ((List<ErrorMessage>)Errors).AddRange(errors);
            }
        }

        public ApiListResult(T content) : this(content, true, null)
        {
        }

        public ApiListResult() : base()
        {
        }

        #region Properties
        public T Data { get; set; }
        public int Size { get; set; }
        public int CountTotal { get; set; }

        #endregion
    }
}
