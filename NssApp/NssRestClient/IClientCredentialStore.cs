using System.Threading.Tasks;

namespace NssRestClient
{
    public interface IClientCredentialStore
    {
        Task<bool> HasCredentialsAsync();
        Task<NssConnectionState> GetAsync();
        Task SetAsync(NssConnectionState nssConnectionState);
        Task Clear();
    };
}
