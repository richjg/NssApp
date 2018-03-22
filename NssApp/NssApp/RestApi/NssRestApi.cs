using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NssApp.RestApi
{
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

        public async Task<List<Machine>> GetComputers(int page, int numberOfMachines, string searchText)
        {
            var machines = new List<Machine>();
            
            var skip = numberOfMachines * (page - 1);
            var result = await HttpClient.GetAsync($"v6/machines?$filter=contains(DisplayName, '{searchText}')&$top={numberOfMachines}&$skip={skip}");
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