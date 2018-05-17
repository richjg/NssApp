namespace NssRestClient
{
    public class RestClientFactory
    {
        public static IRestClient Create() => new RestClient(NssHttpClientFactory.Instance, InMemoryClientCredentialStore.Instance);
        public static IRestClient Create(INssHttpClientFactory httpClientFactory, IClientCredentialStore clientCredentialStore) => new RestClient(httpClientFactory, clientCredentialStore);
    }
}
