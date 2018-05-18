using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NssApp.Dialog;
using NssRestClient.Dto;
using Xamarin.Forms;

namespace NssApp.RestApi
{
    public static class RestResultExtensions
    {
        public static async Task<TResult> Match<T, TResult>(this Task<RestResult<T>> restResultTask, Func<T, TResult> valid, Func<RestResultError, Task> errors, Func<Task> loginRequired)
        {
            var restResult = await restResultTask;

            if (restResult.HasResult)
            {
                return valid(restResult.Result);
            }

            if (restResult.LoginRequired)
            {
                await loginRequired();
                return default(TResult);
            }

            await errors(restResult.RestResultError);
            return default(TResult);
        }

        public static Task<T> ResolveData<T>(this Task<RestResult<T>> restResultTask, Page page) => restResultTask.Match(valid: r => r, errors: e => App.GetHttpErrorMethod(page)(), loginRequired: App.GetLoginFailedMethod(page));

        /// <summary>
        /// If the Result result HasResult is true then the data is returned. If there errors or login is required the return is null and a message will be displayed on screen.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="restResultTask"></param>
        /// <returns></returns>
        public static Task<T> ResolveData<T>(this Task<RestResult<T>> restResultTask)
        {
            var dialogService = ViewModels.Locator.Instance.Resolve<IDialogService>();

            //Maybe we could change this... have IConnectionStateService - that stores (static) connection state issue... then the ui could show an icon on the nav bar ?

            return restResultTask.Match(valid: r => r,
                                errors: e => dialogService.ShowAlertAsync("Hmm having trouble connecting to the server.", "Connecting", "Ok"),
                                loginRequired: () => dialogService.ShowAlertAsync("Hmm having trouble connecting to the server. Goto setting's to take a look", "Connecting", "Ok"));
        }
    }
}