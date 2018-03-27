using System.Collections.Generic;

namespace NssApp.RestApi
{
    public class RestResultError
    {
        public List<RestResultErrorMessage> Messages { get; set; } = new List<RestResultErrorMessage>();
    }
}