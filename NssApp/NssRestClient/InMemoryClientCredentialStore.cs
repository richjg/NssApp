using System.Threading.Tasks;

namespace NssRestClient
{
    public class InMemoryClientCredentialStore : IClientCredentialStore
    {
        public readonly static InMemoryClientCredentialStore Instance = new InMemoryClientCredentialStore();

        private NssConnectionState nssConnectionState = null;

        public Task Clear()
        {
            this.nssConnectionState = null;
            return Task.CompletedTask;
        }
        public Task<NssConnectionState> GetAsync() => Task.FromResult(this.nssConnectionState);

        public Task<bool> HasCredentialsAsync() => Task.FromResult(this.nssConnectionState != null);

        public Task SetAsync(NssConnectionState nssConnectionState)
        {
            this.nssConnectionState = nssConnectionState;
            return Task.CompletedTask;
        }
    }

}
