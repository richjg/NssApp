using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NssApp.RestApi
{
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
