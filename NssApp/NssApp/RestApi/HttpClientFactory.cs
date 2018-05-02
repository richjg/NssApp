using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NssApp.RestApi
{
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
}
