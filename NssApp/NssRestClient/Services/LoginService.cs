using NssRestClient.Dto;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NssRestClient.Services
{
    public class LoginService
    {
        private readonly IRestClient restClient;

        public LoginService(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public Task<bool> SignInAsync(string url, string username, string password) => this.restClient.SignIn(url, username, password);
    }

}
