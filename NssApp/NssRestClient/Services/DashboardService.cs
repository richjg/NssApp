using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NssRestClient.Dto;

namespace NssRestClient.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IRestClient restClient;

        public DashboardService(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public Task<RestResult<ApiTrafficLight>> GetTrafficLightForSystem() => this.restClient.GetAsync<ApiTrafficLight>($"v6/trafficlights/self");
    }
}
