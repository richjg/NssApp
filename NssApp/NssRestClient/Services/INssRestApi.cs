using System.Threading.Tasks;

namespace NssRestClient.Services
{
    public interface INssRestApi
    {
        IMachineService MachineService { get; }

        Task<bool> Login(string baseUrl, string username, string password);
        Task Logout();
    }
}