using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NssRestClient.Dto;

namespace NssRestClient.Services
{
    public class SystemService : ISystemService
    {
        private readonly IRestClient restClient;

        public SystemService(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public Task<RestResult<ApiUser>> GetLoggedInUser() => this.restClient.GetAsync<ApiUser>($"v6/system/user");
    }
}
