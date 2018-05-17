using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NssRestClient.Net
{
    public class RetryHandler : DelegatingHandler
    {
        private const int MaxRetries = 3;

        public RetryHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        { }

        private readonly static System.Net.HttpStatusCode[] HttpStatusCodesWorthRetrying = { System.Net.HttpStatusCode.RequestTimeout, System.Net.HttpStatusCode.InternalServerError, System.Net.HttpStatusCode.BadGateway, System.Net.HttpStatusCode.ServiceUnavailable, System.Net.HttpStatusCode.GatewayTimeout};

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponse = null;
            for (int i = 1; i <= MaxRetries; i++)
            {
                try
                {
                    httpResponse = await base.SendAsync(request, cancellationToken);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        return httpResponse;
                    }

                    if(HttpStatusCodesWorthRetrying.Contains(httpResponse.StatusCode) == false)
                    {
                        return httpResponse;
                    }
                }
                catch (HttpRequestException) when (i < MaxRetries) { }
                catch (TaskCanceledException) when (i < MaxRetries) { }

                if (i < MaxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(3d, i - 1)));
                }
            }

            return httpResponse;
        }
    }
}
