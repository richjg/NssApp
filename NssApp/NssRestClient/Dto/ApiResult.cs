using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class ApiResult<T>
    {
        public T Data { get; set; }
    }
}
