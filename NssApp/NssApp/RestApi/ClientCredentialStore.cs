using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NssRestClient;
using Xamarin.Essentials;

namespace NssApp.RestApi
{
    public class ClientCredentialStore : IClientCredentialStore
    {
        private const string ServiceId = "FO.NSS.APP.V5";
        private static NssConnectionState currentNssConnectionState;
        private Task<string> GetStoredNssConnectionState() => SecureStorage.GetAsync(ServiceId);
        private Task SetStoredCredentials(NssConnectionState nssConnectionState) => SecureStorage.SetAsync(ServiceId, JsonConvert.SerializeObject(nssConnectionState));

        public async Task<bool> HasCredentialsAsync()
        {
            return (await GetAsync()) != null;
        }

        public async Task<NssConnectionState> GetAsync()
        {
            if (currentNssConnectionState != null)
            {
                return currentNssConnectionState;
            }

            var storedNssConnectionState = await GetStoredNssConnectionState();
            if (storedNssConnectionState == null)
                return null;

            var account = JsonConvert.DeserializeObject<NssConnectionState>(storedNssConnectionState);
            if (account != null)
            {
                currentNssConnectionState = account;
            }

            return account;
        }

        public async Task SetAsync(NssConnectionState nssConnectionState)
        {
            await SetStoredCredentials(nssConnectionState);
            currentNssConnectionState = nssConnectionState;
        }

        public async Task Clear()
        {
            await SetAsync(null);
        }
    }
}
