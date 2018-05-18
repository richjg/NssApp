using NssRestClient.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NssApp.ViewModels
{
    public class SplashScreenViewModel : ViewModelBase
    {
        public override async Task InitializeAsync(object navigationData)
        {
            await this.RunWithBusy(async () =>
            {
                await NavigationService.InitializeAsync();
            });
        }
    }
}
