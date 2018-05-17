using System.Net.Http;

namespace NssRestClient
{
    public interface INssHttpClientFactory
    {
        void Release();
        HttpClient Create(string baseUrl);
        HttpClient Create(NssConnectionState nssConnectionState);
    }
}
