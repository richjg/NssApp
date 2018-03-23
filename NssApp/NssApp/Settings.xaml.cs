using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NssApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
		public Settings ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            var creds = UserCredentialStore.Instance.GetCredentials();
            if(creds == null)
            {
                creds = new RestSettings
                {
                    BaseUrl = "https://uat.biomni.com/DevNetBackupNetBackupAdapterPanels/Api/",
                    Password = "",
                    Username = ""
                };
            }
            UrlEntry.Text = creds.BaseUrl;
            UsernameEntry.Text = creds.Username;
            PasswordEntry.Text = creds.Password;

            base.OnAppearing();
        }

        public async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
            {
                await Navigation.PopAsync();
            }
            else
            {
                await Navigation.PushAsync(new DashBoard());
            }
        }

        public async void OnDoneButtonClicked(object sender, EventArgs e)
        {
            NssRestClient.SetupClient(UrlEntry.Text, UsernameEntry.Text, PasswordEntry.Text);
            if (await NssRestClient.Instance.Login() == false)
            {
                MessageLabel.Text = "Login Failed, check your settings";
            }
            else
            {
                UserCredentialStore.Instance.SetCredentials(UrlEntry.Text, UsernameEntry.Text, PasswordEntry.Text);
                if (Navigation.NavigationStack.Count > 1)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    await Navigation.PushAsync(new DashBoard());
                }
            }
        }
    }
}