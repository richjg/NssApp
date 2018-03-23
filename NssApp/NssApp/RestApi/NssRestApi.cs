using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Auth;

namespace NssApp.RestApi
{
    public class RestSettings
    {
        public string BaseUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }

    public class UserCredentialStore
    {
        private const string ServiceId = "FO.NSS.APP.V2";

        private UserCredentialStore()
        {
        }

        public static readonly UserCredentialStore Instance = new UserCredentialStore();

        public bool HasCredentials() => (AccountStore.Create().FindAccountsForService(ServiceId)).FirstOrDefault() != null;

        public RestSettings GetCredentials()
        {
            var account = (AccountStore.Create().FindAccountsForService(ServiceId)).FirstOrDefault();
            if (account == null)
            {
                return null;
            }

            return new RestSettings
            {
                BaseUrl = account.Properties["baseurl"],
                Password = account.Properties["password"],
                Username = account.Username
            };
        }

        public void SetCredentials(string baseurl, string username, string password)
        {
            var accountStore = AccountStore.Create();

            var account = (accountStore.FindAccountsForService(ServiceId)).FirstOrDefault();
            if (account == null)
            {
                account = new Account();
            }
            account.Username = username;
            account.Properties["password"] = password;
            account.Properties["baseurl"] = baseurl;

            accountStore.Save(account, ServiceId);
        }
    }

    public struct RestResultLoginRequired { }

    public class RestResult<T>
    {
        public T Result { get; set; }
        public bool HasResult { get; set; }
        public bool LoginRequired { get; set; }
        public RestResultError RestResultError { get; set; }

        public static implicit operator RestResult<T>(T data) => new RestResult<T> { HasResult = true, Result = data };
        public static implicit operator RestResult<T>(RestResultError error) => new RestResult<T> { HasResult = false, Result = default(T), RestResultError = error };
        public static implicit operator RestResult<T>(RestResultLoginRequired restResultLoginRequired) => new RestResult<T> { HasResult = false, Result = default(T), LoginRequired = true };
    }

    public class RestResultError
    {
        public List<RestResultErrorMessage> Messages { get; set; } = new List<RestResultErrorMessage>();
    }

    public class RestResultErrorMessage
    {
        public string Message { get; set; }
        public string Property { get; set; }
    }

    public static class RestResultExtensions
    {
        public static async Task<TResult> Match<T, TResult>(this Task<RestResult<T>> restResultTask, Func<T, TResult> valid, Action<RestResultError> errors, Action loginRequired) => (await restResultTask).Match(valid, errors, loginRequired);
        public static TResult Match<T, TResult>(this RestResult<T> restResult, Func<T, TResult> valid, Action<RestResultError> errors, Action loginRequired)
        {
            if (restResult.HasResult)
            {
                return valid(restResult.Result);
            }
            if (restResult.LoginRequired)
            {
                loginRequired();
                return default(TResult);
            }

            errors(restResult.RestResultError);

            return default(TResult);
        }


        public async static Task<TResult> Match<T, TResult>(this Task<RestResult<T>> restResultTask, Func<T, TResult> valid, Func<RestResultError, Task> errors, Func<Task> loginRequired)
        {
            var restResult = await restResultTask;

            if (restResult.HasResult)
            {
                return valid(restResult.Result);
            }
            else if (restResult.LoginRequired)
            {
                await loginRequired();
                return default(TResult);
            }
            else
            {
                await errors(restResult.RestResultError);
                return default(TResult);
            }
        }
    }

    public class NssRestClient
    {
        private static HttpClient httpClient;
        private static Settings settings;
        private static bool AuthenticationFailed;

        public static readonly NssRestClient Instance = new NssRestClient();

        public static void SetupClient(string baseurl, string username, string password)
        {
            if(httpClient == null)
            {
                settings = new Settings { BaseUrl = baseurl, Username = username, Password = password };
                httpClient = new HttpClient
                {
                    BaseAddress = new Uri(baseurl)
                };
            }
            else
            {
                if(settings.BaseUrl != baseurl)
                {
                    httpClient = new HttpClient
                    {
                        BaseAddress = new Uri(baseurl)
                    };
                }
                settings.Username = username;
                settings.Password = password;
            }
        }

        public async Task<bool> Login()
        {
            if(httpClient == null)
            {
                return false;
            }

            //Special case here, does not go through the Send methods
            var result = await httpClient.PostAsync("auth/token", new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", settings.Username),
                    new KeyValuePair<string, string>("password", settings.Password)
            })); 

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var loginResponse = await result.Content.FromJsonAsync<LoginResponse>();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.AccessToken);
                AuthenticationFailed = false;
                return true;
            }

            return false;
        }

        public Task<RestResult<List<Machine>>> GetComputers(int page, int numberOfMachines, string searchText)
        {
            return SendGet<List<Machine>>($"v6/machines?$filter=contains(DisplayName, '{searchText}')&$top={numberOfMachines}&$skip={numberOfMachines * (page - 1)}");
        }

        public Task<RestResult<TrafficLightCounts>> GetTrafficLightCounts()
        {
            return SendGet<TrafficLightCounts>($"v6/trafficlights/self");
        }

        private Task<HttpResponseMessage> SendGet(string url) => SendWithAutoLoginRetryAsync(() => new HttpRequestMessage(HttpMethod.Get, url));
        private Task<HttpResponseMessage> SendPost<T>(string url,T jsonPostObject) => SendWithAutoLoginRetryAsync(() => new HttpRequestMessage(HttpMethod.Post, url) { Content = new StringContent(JsonConvert.SerializeObject(jsonPostObject)) });
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

            if(AuthenticationFailed || httpClient == null)
            {
                //Dont want to lock them out.
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            var response = await httpClient.SendAsync(getRequest());
            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if(await this.Login())
                {
                   response = await httpClient.SendAsync(getRequest());
                }
                else
                {
                    AuthenticationFailed = true;
                }
            }

            return response;
        }

        private class Settings
        {
            public string BaseUrl { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}