using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Auth;
using Xamarin.Forms;

namespace NssApp.RestApi
{
    public class UserCredentialStore
    {
        private const string ServiceId = "FO.NSS.APP.V4";
        private static LoginSettings CurrentLoginSettings;

        public UserCredentialStore()
        {
        }

        public bool HasCredentials() => (AccountStore.Create().FindAccountsForService(ServiceId)).FirstOrDefault() != null;

        public LoginSettings GetCredentials()
        {
            if(CurrentLoginSettings != null)
            {
                return CurrentLoginSettings;
            }

            var account = (AccountStore.Create().FindAccountsForService(ServiceId)).FirstOrDefault();
            if (account == null)
            {
                return null;
            }

            return CurrentLoginSettings = new LoginSettings
            {
                BaseUrl = account.Properties["baseurl"],
                Password = account.Properties["password"],
                AccessToken = account.Properties["accesstoken"],
                Username = account.Username
            };
        }

        public void UpdateAccessToken(string accessToken)
        {
            var accountStore = AccountStore.Create();

            var currentAccount = (accountStore.FindAccountsForService(ServiceId)).FirstOrDefault();
            if (currentAccount == null)
            {
                return;
            }
            accountStore.Delete(currentAccount, ServiceId);

            var newAccount = new Account
            {
                Username = currentAccount.Username
            };
            newAccount.Properties["password"] = currentAccount.Properties["password"];
            newAccount.Properties["baseurl"] = currentAccount.Properties["baseurl"];
            newAccount.Properties["accesstoken"] = accessToken;

            if (CurrentLoginSettings != null)
            {
                CurrentLoginSettings.AccessToken = accessToken;
            }

            accountStore.Save(newAccount, ServiceId);
        }

        public void SetCredentials(LoginSettings loginSettings)
        {
            var accountStore = AccountStore.Create();

            var account = (accountStore.FindAccountsForService(ServiceId)).FirstOrDefault();
            if (account != null)
            {
                accountStore.Delete(account, ServiceId);
            }
            account = new Account
            {
                Username = loginSettings.Username
            };
            account.Properties["password"] = loginSettings.Password;
            account.Properties["baseurl"] = loginSettings.BaseUrl;
            account.Properties["accesstoken"] = loginSettings.AccessToken;

            accountStore.Save(account, ServiceId);

            CurrentLoginSettings = loginSettings;
        }
    }

    public struct RestResultLoginRequired { }

    public class RestResult
    {
        public RestResultError RestResultError { get; set; }
    }

    public class RestResult<T>
    {
        public T Result { get; set; }
        public bool HasResult { get; set; }
        public bool LoginRequired { get; set; }
        public RestResultError RestResultError { get; set; }

        public static implicit operator RestResult<T>(T data) => new RestResult<T> { HasResult = true, Result = data };
        public static implicit operator RestResult<T>(RestResultError error) => new RestResult<T> { HasResult = false, Result = default(T), RestResultError = error };
        public static implicit operator RestResult<T>(RestResultLoginRequired restResultLoginRequired) => new RestResult<T> { HasResult = false, Result = default(T), LoginRequired = true };
    }

    public class RestResultError
    {
        public List<RestResultErrorMessage> Messages { get; set; } = new List<RestResultErrorMessage>();
    }

    public class RestResultErrorMessage
    {
        public string Message { get; set; }
        public string Property { get; set; }
    }

    public static class RestResultExtensions
    {
        public static async Task<TResult> Match<T, TResult>(this Task<RestResult<T>> restResultTask, Func<T, TResult> valid, Action<RestResultError> errors, Action loginRequired) => (await restResultTask).Match(valid, errors, loginRequired);
        public static TResult Match<T, TResult>(this RestResult<T> restResult, Func<T, TResult> valid, Action<RestResultError> errors, Action loginRequired)
        {
            if (restResult.HasResult)
            {
                return valid(restResult.Result);
            }
            if (restResult.LoginRequired)
            {
                loginRequired();
                return default(TResult);
            }

            errors(restResult.RestResultError);

            return default(TResult);
        }


        public async static Task<TResult> Match<T, TResult>(this Task<RestResult<T>> restResultTask, Func<T, TResult> valid, Func<RestResultError, Task> errors, Func<Task> loginRequired)
        {
            var restResult = await restResultTask;

            if (restResult.HasResult)
            {
                return valid(restResult.Result);
            }
            else if (restResult.LoginRequired)
            {
                await loginRequired();
                return default(TResult);
            }
            else
            {
                await errors(restResult.RestResultError);
                return default(TResult);
            }
        }

        public static Task<T> ResolveData<T>(this Task<RestResult<T>> restResultTask, Page page) => restResultTask.Match(valid: r => r, errors: (e) => Task.CompletedTask, loginRequired: App.GetLoginFailedMethod(page));
    }
}