using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NssRestClient.Dto;

namespace NssRestClient.Services
{
    public class UtilizationService : IUtilizationService
    {
        private readonly IRestClient restClient;

        public UtilizationService(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public Task<RestResult<List<ApiSystemUtilizationMonth>>> GetSystemUtilisationMonths() => this.restClient.GetAsync<List<ApiSystemUtilizationMonth>>($"v6/utilization/systemmonths");
    }
}
