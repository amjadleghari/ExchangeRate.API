using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRate.Results
{
    public class ApiResult
    {
        public ApiResult()
        {

        }

        public bool Success { get; set; }

        public IEnumerable<ErrorMessage> Errors { get; set; }

        #region Methods

        public void AddError(string message, string code = null)
        {
            if (Errors == null) Errors = new List<ErrorMessage>();
            ((List<ErrorMessage>)Errors).Add(new ErrorMessage() { Code = code, Message = message });
        }

        public List<ErrorMessage> GetErrors()
        {
            return Errors as List<ErrorMessage>;
        }

        #endregion
    }
    public class ApiResult<T> : ApiResult
    {
        public ApiResult(T content, IEnumerable<ErrorMessage> errors)
        {
            Data = content;
            if (errors != null && errors.Count() > 0)
            {
                Errors = new List<ErrorMessage>();
                ((List<ErrorMessage>)Errors).AddRange(errors);
            }
        }

        public ApiResult(T content) : this(content, null)
        {
        }

        public ApiResult() : base()
        {
        }

        #region Properties
        public T Data { get; set; }

        #endregion
    }
}
