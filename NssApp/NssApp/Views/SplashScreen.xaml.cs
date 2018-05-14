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
	public partial class SplashScreen : ContentPage
	{
		public SplashScreen()
		{
			InitializeComponent();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var nssRestService = new NssRestApiService(new UserCredentialStore(), new HttpClientFactory());
            if (await nssRestService.TryAuthReAuthenticate() == true)
            {
                Application.Current.MainPage = new Tabs();
            }
            else
            {
                Application.Current.MainPage = new Settings();
            }
        }
    }
}