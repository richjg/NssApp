using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class RestResult<T>
    {
        public T Result { get; set; }
        public bool HasResult { get; set; }
        public bool LoginRequired { get; set; }
        public RestResultError RestResultError { get; set; }

        public static implicit operator RestResult<T>(T data) => new RestResult<T> { HasResult = true, Result = data };
        public static implicit operator RestResult<T>(RestResultError error) => new RestResult<T> { HasResult = false, Result = default(T), RestResultError = error };
        public static implicit operator RestResult<T>(RestResultLoginRequired restResultLoginRequired) => new RestResult<T> { HasResult = false, Result = default(T), LoginRequired = true };
    }
}
