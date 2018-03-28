using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NssApp.RestApi
{
    public class LoginSettings
    {
        public string BaseUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }
    }

    public class HttpClientFactory
    {
        private static HttpClient httpClient;

        public void SetAuth(LoginSettings loginSettings, string accessToken)
        {

        }

        public HttpClient Create(LoginSettings loginSettings)
        {
            if(loginSettings == null)
            {
                return null;
            }

            if(httpClient == null)
            {
                httpClient = new HttpClient
                {
                    BaseAddress = new Uri(loginSettings.BaseUrl)
                };
            }
            else if(httpClient.BaseAddress.OriginalString != loginSettings.BaseUrl)
            {
                httpClient = new HttpClient
                {
                    BaseAddress = new Uri(loginSettings.BaseUrl)
                };
            }

            if(httpClient.DefaultRequestHeaders.Authorization?.Parameter != loginSettings.AccessToken)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginSettings.AccessToken);
            }

            return httpClient;
        }
    }

    public class NssRestApiService
    {
        public static bool AuthenticationFailed;
        private readonly UserCredentialStore userCredentialStore;
        private readonly HttpClientFactory httpClientFactory;

        public NssRestApiService(UserCredentialStore userCredentialStore, HttpClientFactory httpClientFactory)
        {
            this.userCredentialStore = userCredentialStore;
            this.httpClientFactory = httpClientFactory;
        }

        private async Task<HttpClient> GetRestClient()
        {
             return this.httpClientFactory.Create(await this.GetLoginSettings());
        }

        private LoginSettings _loginSettings;
        private Task<LoginSettings> GetLoginSettings()
        {
            if (this._loginSettings == null)
            {
                this._loginSettings = this.userCredentialStore.GetCredentials();
            }

            return Task.FromResult(this._loginSettings);
        }

        public async Task<LoggedInUserInfo> GetCurrentUserInfo()
        {
            var result = await this.SendGet<LoggedInUserInfo>("v6/system/user");

            if (result.HasResult)
            {
                return result.Result;
            }
            return null;
        }

        public Task<RestResult<List<Machine>>> GetComputers(int page, int numberOfMachines, string searchText)
        {
            return SendGet<List<Machine>>($"v6/machines?$filter=contains(DisplayName, '{searchText}')&$top={numberOfMachines}&$skip={numberOfMachines * (page - 1)}");
        }

        public Task<RestResult<TrafficLightCounts>> GetTrafficLightCounts()
        {
            return SendGet<TrafficLightCounts>($"v6/trafficlights/self");
        }

        public Task<RestResult<MachineProtection>> GetMachineProtection(int machineId)
        {
            return SendGet<MachineProtection>($"v6/machines/{machineId}/protected");
        }

        public Task<RestResult<List<ApiProtectionLevel>>> GetAvailableMachineProtectionLevels(int machineId)
        {
            return SendGet<List<ApiProtectionLevel>> ($"v6/machines/{machineId}/protection/levels");
        }

        private Task<HttpResponseMessage> SendGet(string url) => SendWithAutoLoginRetryAsync(() => new HttpRequestMessage(HttpMethod.Get, url));
        private Task<HttpResponseMessage> SendPost<T>(string url, T jsonPostObject) => SendWithAutoLoginRetryAsync(() => new HttpRequestMessage(HttpMethod.Post, url) { Content = new StringContent(JsonConvert.SerializeObject(jsonPostObject)) });
        private Task<HttpResponseMessage> SendPut<T>(string url, T jsonPostObject) => SendWithAutoLoginRetryAsync(() => new HttpRequestMessage(HttpMethod.Put, url) { Content = new StringContent(JsonConvert.SerializeObject(jsonPostObject)) });
        private Task<HttpResponseMessage> SendDelete(string url) => SendWithAutoLoginRetryAsync(() => new HttpRequestMessage(HttpMethod.Delete, url));

        private async Task<RestResult<T>> SendGet<T>(string url)
        {
            var result = await SendGet(url);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiResult = await result.Content.FromJsonAsync<ApiResult<T>>();
                return apiResult.Data;
            }

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new RestResultLoginRequired();
            }

            return new RestResultError { Messages = { new RestResultErrorMessage { Message = result.ReasonPhrase } } };
        }

        private async Task<HttpResponseMessage> SendWithAutoLoginRetryAsync(Func<HttpRequestMessage> getRequest)
        {
            await Task.Yield();

            var client = await this.GetRestClient();

            if (AuthenticationFailed || client == null)
            {
                //Dont want to lock them out.
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            var response = await client.SendAsync(getRequest());
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if ((await this.TryAuthReAuthenticate()))
                {
                    client = await this.GetRestClient();
                    response = await client.SendAsync(getRequest());
                }
                else
                {
                    AuthenticationFailed = true;
                }
            }

            return response;
        }

        public async Task<bool> TryAuthReAuthenticate()
        {
            var loginSettings = await this.GetLoginSettings();

            if(loginSettings == null)
            {
                return false;
            }

            var client = await this.GetRestClient();

            //Special case here, does not go through the Send methods
            var result = await client.PostAsync("auth/token", new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", loginSettings.Username),
                    new KeyValuePair<string, string>("password", loginSettings.Password)
            }));

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var loginResponse = await result.Content.FromJsonAsync<LoginResponse>();
                AuthenticationFailed = false;
                this.userCredentialStore.UpdateAccessToken(loginResponse.AccessToken);
                this._loginSettings = null;
                return true;
            }

            return false;
        }

        public async Task<bool> SignIn(string url, string username, string password)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(url)
                };

                //Special case here, does not go through the Send methods
                var result = await client.PostAsync("auth/token", new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
            }));

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var loginResponse = await result.Content.FromJsonAsync<LoginResponse>();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.AccessToken);

                    ///v6/system/user
                    var userInfoReponse = await client.GetAsync("v6/system/user");
                    var loggedInUserInfo = await userInfoReponse.Content.FromJsonAsync<ApiResult<LoggedInUserInfo>>();
                    this.userCredentialStore.SetCredentials(new LoginSettings { AccessToken = loginResponse.AccessToken, BaseUrl = url, Password = password, Username = username });
                    NssRestApiService.AuthenticationFailed = false;
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

    }
}
