using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NssApp.RestApi
{
    public class RetryHandler : DelegatingHandler
    {
        private const int MaxRetries = 3;

        public RetryHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
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
                }
                catch (HttpRequestException) when (i < MaxRetries) { }
                catch (TaskCanceledException) when (i < MaxRetries) { }

                if (i < MaxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(3d, i-1)));
                }
            }

            return httpResponse;
        }
    }
}
