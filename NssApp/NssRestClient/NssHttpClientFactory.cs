using NssRestClient.Net;
using System;
using System.Net.Http;

namespace NssRestClient
{
    public class NssHttpClientFactory : INssHttpClientFactory
    {
        private static HttpClient httpClient;

        public readonly static NssHttpClientFactory Instance = new NssHttpClientFactory();

        public HttpClient Create(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl) || Uri.IsWellFormedUriString(baseUrl, UriKind.RelativeOrAbsolute) == false)
            {
                throw new ArgumentException("A valid url is required", nameof(baseUrl));
            }

            if (httpClient == null || httpClient.BaseAddress.OriginalString != baseUrl)
            {
                httpClient = new HttpClient(new RetryHandler(new HttpClientHandler()))
                {
                    BaseAddress = new Uri(baseUrl)
                };
            }
            return httpClient;
        }

        public HttpClient Create(NssConnectionState nssConnectionState) => Create(nssConnectionState.BaseUrl);

        public void Release()
        {
            try
            {
                httpClient?.Dispose();
            }
            catch { }

            httpClient = null;
        }
    }
}
