using System.Threading.Tasks;

namespace NssRestClient.Services
{
    public class NssRestApi : INssRestApi
    {
        private readonly IRestClient restClient;

        public NssRestApi(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public static NssRestApi Create() => new NssRestApi(RestClientFactory.Create());

        public Task<bool> Login(string baseUrl, string username, string password) => this.restClient.SignIn(baseUrl, username, password);
        public Task Logout() => restClient.SignOut();

        private IMachineService machineService;
        public IMachineService MachineService
        {
            get
            {
                if(machineService == null)
                {
                    machineService = new MachineService(this.restClient);
                }

                return machineService;
            }
        } 
    }
}
