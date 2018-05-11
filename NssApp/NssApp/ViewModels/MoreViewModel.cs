using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NssApp.ViewModels
{
    public class MoreViewModel
    {
        private readonly LoggedInUserService _loggedInUserService;

        public MoreViewModel(LoggedInUserService loggedInUserService)
        {
            this._loggedInUserService = loggedInUserService;
        }

        public MoreViewModel Initialize(Page page)
        {
            return this;
        }
        
        public ICommand LogoutCommand { get => new Command(async () => await Logout()); }

        private async Task Logout()
        {
            var x = await _loggedInUserService.TryGetLoggedInUserSettings();

            _loggedInUserService.SetCredentials(new LoginSettings
            {
                BaseUrl = x.BaseUrl,
                Username = "",
                Password = "",
                AccessToken = ""
            });

            Application.Current.MainPage = new SplashScreen();
        }
    }
}
