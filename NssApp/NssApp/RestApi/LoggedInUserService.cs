using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NssApp.RestApi
{
    public class LoggedInUserService
    {
        private readonly NssRestApiService nssRestApiService;
        private readonly UserCredentialStore userCredentialStore;

        public LoggedInUserService(NssRestApiService nssRestApiService, UserCredentialStore userCredentialStore)
        {
            this.nssRestApiService = nssRestApiService;
            this.userCredentialStore = userCredentialStore;
        }

        public Task<LoginSettings> TryGetLoggedInUserSettings()
        {
            return Task.FromResult(this.userCredentialStore.GetCredentials());
        }
        
        public void SetCredentials(LoginSettings loginSettings)
        {
            this.userCredentialStore.SetCredentials(loginSettings);
        }

        public Task<bool> SignIn(string url, string username, string password) => this.nssRestApiService.SignIn(url, username, password);
    }
}
