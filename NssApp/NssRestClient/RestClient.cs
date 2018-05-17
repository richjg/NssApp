using Newtonsoft.Json;
using NssRestClient.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NssRestClient
{
    public class RestClient : IRestClient
    {
        private readonly INssHttpClientFactory httpClientFactory;
        private readonly IClientCredentialStore clientCredentialStore;

        public RestClient(INssHttpClientFactory httpClientFactory, IClientCredentialStore clientCredentialStore)
        {
            this.httpClientFactory = httpClientFactory;
            this.clientCredentialStore = clientCredentialStore;
        }
        public Task<bool> SignIn(string baseUrl, string username, string password)
        {
            this.httpClientFactory.Release();
            this.clientCredentialStore.Clear();
            return this.TryAuthenticate(this.httpClientFactory.Create(baseUrl), new NssConnectionState { BaseUrl = baseUrl, Username = username, Password = password });
        }
        public Task SignOut() => clientCredentialStore.Clear();

        public Task<RestResult<T>> GetAsync<T>(string url) => HandleResponse<T>(GetAsync(url));
        public Task<RestResult<T>> PostJsonAsync<T>(string url, object postData) => HandleResponse<T>(PostJsonAsync(url, postData));
        public Task<RestResult<T>> PutJsonAsync<T>(string url, object postData) => HandleResponse<T>(PutJsonAsync(url, postData));
        public Task<RestResult<T>> DeleteAsync<T>(string url) => HandleResponse<T>(DeleteAsync(url));
        public async Task<LoginSettings> GetCurrentLoginSettings()
        {
            var nssConnectionState = await this.clientCredentialStore.GetAsync();
            if (nssConnectionState == null)
                return null;

            return new LoginSettings
            {
                BaseUrl = nssConnectionState.BaseUrl,
                Username = nssConnectionState.Username
            };
        }

        private Task<HttpResponseMessage> GetAsync(string url) => SendWithAutoLoginRetryAsync(() => new HttpRequestMessage(HttpMethod.Get, url));
        private Task<HttpResponseMessage> PostJsonAsync(string url, object jsonPostObject) => SendWithAutoLoginRetryAsync(() => new HttpRequestMessage(HttpMethod.Post, url) { Content = new StringContent(JsonConvert.SerializeObject(jsonPostObject) ?? string.Empty, System.Text.Encoding.UTF8, "application/json") });
        private Task<HttpResponseMessage> PutJsonAsync(string url, object jsonPostObject) => SendWithAutoLoginRetryAsync(() => new HttpRequestMessage(HttpMethod.Put, url) { Content = new StringContent(JsonConvert.SerializeObject(jsonPostObject) ?? string.Empty, System.Text.Encoding.UTF8, "application/json") });
        private Task<HttpResponseMessage> DeleteAsync(string url) => SendWithAutoLoginRetryAsync(() => new HttpRequestMessage(HttpMethod.Delete, url));
        private async Task<HttpResponseMessage> SendWithAutoLoginRetryAsync(Func<HttpRequestMessage> getRequest)
        {
            await Task.Yield();

            var nssConnectionState = await this.clientCredentialStore.GetAsync();
            if (nssConnectionState == null)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "No client credetials setup, login first" };
            }

            if (nssConnectionState.RetryAuthenticateFailed)
            {
                //Dont want to lock them out.
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized) { ReasonPhrase = "Password might have changed, try login again" };
            }

            HttpResponseMessage response;
            try
            {
                var httpClient = this.httpClientFactory.Create(nssConnectionState);
                response = await httpClient.SendAsync(GetHttpRequestMessage());
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if ((await this.TryAuthenticate(httpClient, nssConnectionState)))
                    {
                        httpClient = this.httpClientFactory.Create(nssConnectionState);
                        response = await httpClient.SendAsync(GetHttpRequestMessage());
                    }
                }
            }
            catch (Exception e)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.ServiceUnavailable, ReasonPhrase = e.Message };
            }

            return response;

            HttpRequestMessage GetHttpRequestMessage()
            {
                var requestMessage = getRequest();
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", nssConnectionState.AccessToken);
                return requestMessage;
            }
        }
        private async Task<RestResult<T>> HandleResponse<T>(Task<HttpResponseMessage> httpResponseMessage)
        {
            var result = await httpResponseMessage;

            if (result.IsSuccessStatusCode)
            {
                var apiResult = await result.ContentFromJsonAsync<ApiResult<T>>();
                return apiResult.Data;
            }

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new RestResultLoginRequired();
            }

            return new RestResultError { Messages = { new RestResultErrorMessage { Message = result.ReasonPhrase } } };
        }
        private async Task<bool> TryAuthenticate(HttpClient client, NssConnectionState nssConnection)
        {
            try
            {
                if (nssConnection == null)
                {
                    return false;
                }

                //Special case here, does not go through the Send methods
                var httpResponseMessage = await client.PostAsync("auth/token", new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", nssConnection.Username),
                    new KeyValuePair<string, string>("password", nssConnection.Password)
                }));

                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var loginResponse = await httpResponseMessage.ContentFromJsonAsync<LoginResponse>();
                    nssConnection.AccessToken = loginResponse.AccessToken;
                    nssConnection.RetryAuthenticateFailed = false;
                    await this.clientCredentialStore.SetAsync(nssConnection);
                    return true;
                }
                else
                {
                    nssConnection.RetryAuthenticateFailed = true;
                    await this.clientCredentialStore.SetAsync(nssConnection);
                }
            }
            catch { }

            return false;
        }
    }
}
