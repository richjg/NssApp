using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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

    public class Machine
    {
        public string DisplayName { get; set; }
        public string TrafficLightStatus { get; set; }

        public string BackgroundColor
        {
            get
            {
                switch (TrafficLightStatus)
                {
                    case "Red":
                        return "Red";
                    case "Amber":
                        return "Orange";
                    case "Green":
                        return "Green";
                    default:
                        break;
                }

                return "";
            }
        }
    }

    public class TrafficLightCounts
    {
        public string RedCount { get; set; }
        public string AmberCount { get; set; }
        public string GreenCount { get; set; }
    }

    public class ApiResult<T>
    {
        public T Data { get; set; }
    }

    public class NssRestApi
    {
        private static readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://uat.biomni.com/DevNetBackupNetBackupAdapterPanels/Api/")
        };

        private static LoginResponse LoginResponse { get; set; }

        public async Task<bool> Login(string username, string password)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; };

                var result = await HttpClient.PostAsync("auth/token", new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                }));

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonText = await result.Content.ReadAsStringAsync();
                    LoginResponse = JsonConvert.DeserializeObject<LoginResponse>(jsonText);
                    HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", LoginResponse.AccessToken);
                    return true;
                }
            }
            catch (Exception e)
            {
                Exception ee = e;
            }

            return false;
        }

        public async Task<List<Machine>> GetComputers()
        {
            var machines = new List<Machine>();

            var result = await HttpClient.GetAsync("v6/machines?$top=20&$skip=0"); //;?$filter=CustomerCode eq 'Acme'&$top=10&$skip=0&$count=true");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonText = await result.Content.ReadAsStringAsync();
                var apiResult = JsonConvert.DeserializeObject<ApiResult<List<Machine>>>(jsonText);
                machines = apiResult.Data;
            }

            return machines;
        }

        public async Task<TrafficLightCounts> GetTrafficLightCounts()
        {
            var trafficLightCounts = new TrafficLightCounts();

            var result = await HttpClient.GetAsync("v6/trafficlights/self");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonText = await result.Content.ReadAsStringAsync();
                var apiResult = JsonConvert.DeserializeObject<ApiResult<TrafficLightCounts>>(jsonText);
                trafficLightCounts = apiResult.Data;
            }

            return trafficLightCounts;
        }
    }
}