using System;
using System.Collections.Generic;
using System.Text;

namespace NssApp.RestApi
{
    public class LoginSettings
    {
        public string BaseUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }
    }
}
