using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NssApp.RestApi
{
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

        public Task<RestResult<List<MachineImage>>> GetMachineImages(int machineId)
        {
            return SendGet<List<MachineImage>>($"v6/machines/{machineId}/backupimages");
        }

        public Task<RestResult<List<MachineUtilisationMonth>>> GetMachineUtilisationMonths(int machineId)
        {
            return SendGet<List<MachineUtilisationMonth>>($"v6/utilization/machinemonths?$filter=MachineId eq {machineId}");
        }

        public Task<RestResult<List<SystemUtilisationMonth>>> GetSystemUtilisationMonths()
        {
            return SendGet<List<SystemUtilisationMonth>>($"v6/utilization/systemmonths");
        }

        public Task<RestResult<Activity>> ProtectMachine(int machineId, int protectionLevelId)
        {
            return SendPost<Activity>($"v6/machines/{machineId}/protect", new { protectionLevelId });
        }

        public async Task<RestResult<List<Activity<ProtectMachineActivityData>>>> GetExecutingProtectMachineActivities(int machineId)
        {
            var restResult = await SendGet<List<Activity<ProtectMachineActivityData>>>($"v6/activities?$filter=Type eq 'NetBackupSelfService.Tasks.ProtectMachineTask' and EntityType eq 'Machine' and EntityKey eq {machineId} and (Status eq 'Pending' or Status eq 'Queued' or Status eq 'Running')");
            if(restResult.HasResult)
            {
                var restResultApiProtectionLevels = await this.GetAvailableMachineProtectionLevels(machineId);
                if(restResultApiProtectionLevels.HasResult == false)
                {
                    return new List<Activity<ProtectMachineActivityData>>();
                }

                var apiProtectionLevels = restResultApiProtectionLevels.Result;

                foreach (var item in restResult.Result)
                {
                    item.ActivityData = item.Data.FromJson<ProtectMachineActivityData>();
                    if (item.ActivityData != null)
                    {
                        item.ActivityData.ApiProtectionLevel = apiProtectionLevels.FirstOrDefault(l => l.Id == item.ActivityData.ProtectionLevelId);
                    }
                }
            }

            return restResult;
        }



        private Task<HttpResponseMessage> SendGet(string url) => SendWithAutoLoginRetryAsync(() => new HttpRequestMessage(HttpMethod.Get, url));
        private Task<HttpResponseMessage> SendPost(string url, object jsonPostObject) => SendWithAutoLoginRetryAsync(() => new HttpRequestMessage(HttpMethod.Post, url) { Content = new StringContent(JsonConvert.SerializeObject(jsonPostObject) ?? string.Empty, System.Text.Encoding.UTF8, "application/json") });
        private Task<HttpResponseMessage> SendPut(string url, object jsonPostObject) => SendWithAutoLoginRetryAsync(() => new HttpRequestMessage(HttpMethod.Put, url) { Content = new StringContent(JsonConvert.SerializeObject(jsonPostObject) ?? string.Empty, System.Text.Encoding.UTF8, "application/json") });
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

        private async Task<RestResult<T>> SendPost<T>(string url, object postData)
        {
            var result = await SendPost(url, postData);

            if (result.StatusCode == System.Net.HttpStatusCode.Accepted)
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

            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(getRequest());
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
            }
            catch (Exception e)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.ServiceUnavailable, ReasonPhrase = e.Message };
            }

            return response;
        }

        public async Task<bool> TryAuthReAuthenticate()
        {
            try
            {
                var loginSettings = await this.GetLoginSettings();

                if (loginSettings == null)
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
                    loginSettings.AccessToken = loginResponse.AccessToken;
                    AuthenticationFailed = false;
                    this.userCredentialStore.SetCredentials(loginSettings);
                    this._loginSettings = null;
                    return true;
                }
            }
            catch
            {

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
