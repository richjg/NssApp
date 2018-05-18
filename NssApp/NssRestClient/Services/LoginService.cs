using NssRestClient.Dto;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NssRestClient.Services
{
    public class LoginService : ILoginService
    {
        private readonly IRestClient restClient;

        public LoginService(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public Task<bool> SignInAsync(string url, string username, string password) => this.restClient.SignIn(url, username, password);

        public Task SignOutAsync() => this.restClient.SignOut();

        public Task<LoginSettings> GetCurrentLoginSettings() => this.restClient.GetCurrentLoginSettings();

        public async Task<bool> UserIsAuthenticatedAndValidAsync()
        {
            var result = await this.restClient.GetAsync<ApiUser>($"v6/system/user");
            return result.LoginRequired == false;
        }
    }
}
