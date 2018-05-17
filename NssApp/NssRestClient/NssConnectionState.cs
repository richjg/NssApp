using System;

namespace NssRestClient
{
    public class NssConnectionState
    {
        public string BaseUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }
        public bool RetryAuthenticateFailed { get; set; }
    }

}
