﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}