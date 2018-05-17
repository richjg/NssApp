using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NssRestClient.Services;
using Xamarin.Forms;

namespace NssApp.ViewModels
{
    public class MoreViewModel
    {
        private readonly LoginService loginService;

        public MoreViewModel(LoginService loginService)
        {
            this.loginService = loginService;
        }

        public MoreViewModel Initialize(Page page)
        {
            return this;
        }
        
        public ICommand LogoutCommand => new Command(async () => await Logout());

        private async Task Logout()
        {
            await loginService.SignOutAsync();
            Application.Current.MainPage = new SplashScreen();
        }
    }
}
