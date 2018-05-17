using System.Collections.Generic;
using System.Threading.Tasks;
using NssRestClient.Dto;

namespace NssRestClient.Services
{
    public interface IMachineService
    {
        Task<RestResult<List<ApiMachine>>> GetComputers(int page, int numberOfMachines, string searchText);
        Task<RestResult<ApiProtected>> GetMachineProtection(int machineId);
        Task<RestResult<List<ApiProtectionLevel>>> GetAvailableMachineProtectionLevels(int machineId);
        Task<RestResult<List<ApiBackupImage>>> GetMachineImages(int machineId);
        Task<RestResult<List<ApiMachineUtilisationMonth>>> GetMachineUtilisationMonths(int machineId);
    }
}