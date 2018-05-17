using NssRestClient.Dto;
using System.Threading.Tasks;

namespace NssRestClient
{
    public interface IRestClient
    {
        Task SignOut();
        Task<bool> SignIn(string url, string username, string password);
        Task<RestResult<T>> GetAsync<T>(string url);
        Task<RestResult<T>> PostJsonAsync<T>(string url, object postData);
        Task<RestResult<T>> PutJsonAsync<T>(string url, object postData);
        Task<RestResult<T>> DeleteAsync<T>(string url);
    }
}
