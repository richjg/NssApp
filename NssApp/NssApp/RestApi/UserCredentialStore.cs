using System;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace NssApp.RestApi
{
    public class UserCredentialStore
    {
        private const string ServiceId = "FO.NSS.APP.V4";
        private static LoginSettings CurrentLoginSettings;
        
        public LoginSettings GetCredentials()
        {
            if (CurrentLoginSettings != null)
            {
                return CurrentLoginSettings;
            }

            var storedCredentials = GetStoredCredentials();

            if (storedCredentials == null)
                return null;

            var account = JsonConvert.DeserializeObject<LoginSettings>(storedCredentials);
            if (account == null)
            {
                return null;
            }

            return CurrentLoginSettings = new LoginSettings
            {
                BaseUrl = account.BaseUrl,
                Password = account.Password,
                AccessToken = account.AccessToken,
                Username = account.Username
            };
        }

        private string GetStoredCredentials() => SecureStorage.GetAsync(ServiceId).GetAwaiter().GetResult();
        private void SetStoredCredentials(LoginSettings loginSettings) => SecureStorage.SetAsync(ServiceId, JsonConvert.SerializeObject(loginSettings));
        
        public void SetCredentials(LoginSettings loginSettings)
        {
            SetStoredCredentials(loginSettings);
            CurrentLoginSettings = loginSettings;
        }
    }
}