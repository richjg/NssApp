using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace NssApp
{
	public partial class App : Application
	{

        public App ()
		{
			InitializeComponent();

            var creds = UserCredentialStore.Instance.GetCredentials();

            if (creds != null)
            {
                NssRestClient.SetupClient(creds.BaseUrl, creds.Username, creds.Password);
            }

            MainPage = new NavigationPage(new Master());
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
