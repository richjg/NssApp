using NssRestClient.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NssRestClient.Services
{
    public class MachineService : IMachineService
    {
        private readonly IRestClient restClient;

        public MachineService(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public static MachineService Create(IRestClient restClient) => new MachineService(restClient);

        public Task<RestResult<List<ApiMachine>>> GetComputers(int page, int numberOfMachines, string searchText) => this.restClient.GetAsync<List<ApiMachine>>($"v6/machines?$filter=contains(DisplayName, '{searchText}')&$top={numberOfMachines}&$skip={numberOfMachines * (page - 1)}");
        public Task<RestResult<ApiProtected>> GetMachineProtection(int machineId) => this.restClient.GetAsync<ApiProtected>($"v6/machines/{machineId}/protected");
        public Task<RestResult<List<ApiProtectionLevel>>> GetAvailableMachineProtectionLevels(int machineId) => this.restClient.GetAsync<List<ApiProtectionLevel>>($"v6/machines/{machineId}/protection/levels");
        public Task<RestResult<List<ApiBackupImage>>> GetMachineImages(int machineId) => this.restClient.GetAsync<List<ApiBackupImage>>($"v6/machines/{machineId}/backupimages");
        public Task<RestResult<List<ApiMachineUtilisationMonth>>> GetMachineUtilisationMonths(int machineId) => this.restClient.GetAsync<List<ApiMachineUtilisationMonth>>($"v6/utilization/machinemonths?$filter=MachineId eq {machineId}");
        public Task<RestResult<ApiActivity>> ProtectMachine(int machineId, int protectionLevelId) => this.restClient.PostJsonAsync<ApiActivity>($"v6/machines/{machineId}/protect", new { protectionLevelId });
    }
}
