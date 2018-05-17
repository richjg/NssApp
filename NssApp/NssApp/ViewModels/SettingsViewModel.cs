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
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Page _CurrentPage;
        private INavigation _Navigation;
        private readonly LoginService _loginService;


        public SettingsViewModel(LoginService loggedInUserService)
        {
            this._loginService = loggedInUserService;
        }

        public SettingsViewModel Initialize(Page page)
        {
            this._CurrentPage = page;
            this._Navigation = page.Navigation;
            page.Appearing += this.PageOnAppearing;
            return this;
        }

        private void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void SetPropertyValue<T>(ref T t, T value, string name)
        {
            t = value;
            OnPropertyChanged(name);
        }

        private string _url;
        public string Url { get => this._url; set => this.SetPropertyValue(ref _url, value, nameof(Url)); }

        private string _username;
        public string Username { get => this._username; set => this.SetPropertyValue(ref _username, value, nameof(Username)); }

        private string _password;
        public string Password { get => this._password; set => this.SetPropertyValue(ref _password, value, nameof(Password)); }

        private string _loginFailedMessage;
        public string LoginFaileMessage { get => this._loginFailedMessage; set => this.SetPropertyValue(ref _loginFailedMessage, value, nameof(LoginFaileMessage)); }
        
        public ICommand DoneEditSettingsCommand { get => new Command(async () => await DoneEditingSettings()); }

        private async void PageOnAppearing(object sender, EventArgs e)
        {
            try
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
            }
            catch (Exception er)
            {
                Exception ee = er;
            }
        }

        private async Task DoneEditingSettings()
        {
            this.LoginFaileMessage = string.Empty;
            var signedInOK = await this._loginService.SignInAsync(this.Url, this.Username, this.Password);
            if(signedInOK)
            {
                Application.Current.MainPage = new Tabs();
            }
            else
            {
                this.LoginFaileMessage = "Login Failed, check your settings";
            }
        }
    }
}
