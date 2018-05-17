using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class RestResultError
    {
        public List<RestResultErrorMessage> Messages { get; set; } = new List<RestResultErrorMessage>();
    }
}
