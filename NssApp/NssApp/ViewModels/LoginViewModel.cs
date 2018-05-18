using NssRestClient.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NssApp.ViewModels
{
    public class LoginViewModel : ViewModelBase, IHandleViewAppearing
    {
        private readonly ILoginService _loginService;

        public LoginViewModel(ILoginService loginService)
        {
            this._loginService = loginService;
        }


        private string _url;
        public string Url
        {
            get => this._url;
            set
            {
                this._url = value;
                this.OnPropertyChanged();
            }
        }

        private string _userName;
        public string Username
        {
            get => this._userName;
            set
            {
                this._userName = value;
                this.OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => this._password;
            set
            {
                this._password = value;
                this.OnPropertyChanged();
            }
        }

        private string _loginFailedMessage;
         public string LoginFailedMessage
        {
            get => this._loginFailedMessage;
            set
            {
                this._loginFailedMessage = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand SignInCommand => new AsyncCommand(SignInAsync);

        private async Task SignInAsync()
        {
            await RunWithBusy(async () =>
            {
                this.LoginFailedMessage = "";
                if (await this._loginService.SignInAsync(this.Url, this.Username, this.Password))
                {
                    await NavigationService.NavigateToAsync<MainViewModel>();
                }
                else
                {
                    this.LoginFailedMessage = "Login Failed, check your settings";
                }
            });
        }

        public async Task OnViewAppearingAsync(VisualElement view)
        {
            await RunWithBusy(async () =>
            {
                var loginSettings = await _loginService.GetCurrentLoginSettings();
                if (loginSettings != null)
                {
                    this.Url = loginSettings.BaseUrl;
                    this.Username = loginSettings.Username;
                    this.Password = "";
                }
                else
                {
                    this.Url = "https://uat.biomni.com/DevNetBackupNetBackupAdapterPanels/Api/";
                }
            });
        }
    }
}
