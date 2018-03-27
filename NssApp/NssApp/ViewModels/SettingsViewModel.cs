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
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Page _CurrentPage;
        private INavigation _Navigation;
        private readonly LoggedInUserService _loggedInUserService;


        public SettingsViewModel(LoggedInUserService loggedInUserService)
        {
            this._loggedInUserService = loggedInUserService;
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

        public ICommand CancelEditSettingsCommand { get => new Command(async () => await CancelEditSettings()); }
        public ICommand DoneEditSettingsCommand { get => new Command(async () => await DoneEditingSettings()); }

        private async void PageOnAppearing(object sender, EventArgs e)
        {
            try
            {
                var loginSettings = await _loggedInUserService.TryGetLoggedInUserSettings();
                if (loginSettings != null)
                {
                    this.Url = loginSettings.BaseUrl;
                    this.Username = loginSettings.Username;
                    this.Password = loginSettings.Password;
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

        private async Task CancelEditSettings()
        {
            this._Navigation.InsertPageBefore(new DashBoard(), this._CurrentPage);
            await this._Navigation.PopAsync();
        }

        private async Task DoneEditingSettings()
        {
            this.LoginFaileMessage = string.Empty;
            var signedInOK = await this._loggedInUserService.SignIn(this.Url, this.Username, this.Password);
            if(signedInOK)
            {
                this._Navigation.InsertPageBefore(new DashBoard(), this._CurrentPage);
                await this._Navigation.PopAsync();
            }
            else
            {
                this.LoginFaileMessage = "Login Failed, check your settings";
            }
        }
    }
}
