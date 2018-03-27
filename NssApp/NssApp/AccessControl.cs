using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NssApp
{
    public class AccessControl
    {
        private static async Task CheckLoggedInUserInfo()
        {
            if (App.LoggedInUserInfo == null)
            {
                App.LoggedInUserInfo = await NssRestClient.Instance.GetCurrentUserInfo();
            }
        }

        public async static Task<bool> CanSeeCharts()
        {
            await CheckLoggedInUserInfo();

            if (App.LoggedInUserInfo == null)
            {
                return false;
            }

            return App.LoggedInUserInfo.IsMsp || App.LoggedInUserInfo.IsTenantAdmin;
        }
    }
}
