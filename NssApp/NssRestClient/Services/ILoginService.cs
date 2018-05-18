using System.Threading.Tasks;
using NssRestClient.Dto;

namespace NssRestClient.Services
{
    public interface ILoginService
    {
        Task<LoginSettings> GetCurrentLoginSettings();
        Task<bool> SignInAsync(string url, string username, string password);
        Task SignOutAsync();
        Task<bool> UserIsAuthenticatedAndValidAsync();
    }
}