using System;
using Newtonsoft.Json;

namespace NssApp.RestApi
{
    public class LoginResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("userGuid")]
        public string UserGuid { get; set; }

        [JsonProperty(".issued")]
        public DateTime Issued { get; set; }

        [JsonProperty(".expires")]
        public DateTime Expires { get; set; }
    }

    public class LoggedInUserInfo
    {
        public string CustomerCode { get; set; }
        public bool IsMsp { get; set; }
        public bool IsTenantAdmin { get; set; }
        public int TenantId { get; set; }
        public string UserGuid { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}