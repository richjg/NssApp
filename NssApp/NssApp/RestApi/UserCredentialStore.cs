using System.Linq;
using Xamarin.Auth;

namespace NssApp.RestApi
{
    public class UserCredentialStore
    {
        private const string ServiceId = "FO.NSS.APP.V4";
        private static LoginSettings CurrentLoginSettings;

        public UserCredentialStore()
        {
        }

        public bool HasCredentials() => (AccountStore.Create().FindAccountsForService(ServiceId)).FirstOrDefault() != null;

        public LoginSettings GetCredentials()
        {
            if(CurrentLoginSettings != null)
            {
                return CurrentLoginSettings;
            }

            var account = (AccountStore.Create().FindAccountsForService(ServiceId)).FirstOrDefault();
            if (account == null)
            {
                return null;
            }

            return CurrentLoginSettings = new LoginSettings
            {
                BaseUrl = account.Properties["baseurl"],
                Password = account.Properties["password"],
                AccessToken = account.Properties["accesstoken"],
                Username = account.Username
            };
        }

        public void UpdateAccessToken(string accessToken)
        {
            var accountStore = AccountStore.Create();

            var currentAccount = (accountStore.FindAccountsForService(ServiceId)).FirstOrDefault();
            if (currentAccount == null)
            {
                return;
            }
            accountStore.Delete(currentAccount, ServiceId);

            var newAccount = new Account
            {
                Username = currentAccount.Username
            };
            newAccount.Properties["password"] = currentAccount.Properties["password"];
            newAccount.Properties["baseurl"] = currentAccount.Properties["baseurl"];
            newAccount.Properties["accesstoken"] = accessToken;

            if (CurrentLoginSettings != null)
            {
                CurrentLoginSettings.AccessToken = accessToken;
            }

            accountStore.Save(newAccount, ServiceId);
        }

        public void SetCredentials(LoginSettings loginSettings)
        {
            var accountStore = AccountStore.Create();

            var account = (accountStore.FindAccountsForService(ServiceId)).FirstOrDefault();
            if (account != null)
            {
                accountStore.Delete(account, ServiceId);
            }
            account = new Account
            {
                Username = loginSettings.Username
            };
            account.Properties["password"] = loginSettings.Password;
            account.Properties["baseurl"] = loginSettings.BaseUrl;
            account.Properties["accesstoken"] = loginSettings.AccessToken;

            accountStore.Save(account, ServiceId);

            CurrentLoginSettings = loginSettings;
        }
    }
}