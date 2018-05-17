using System;
using System.Collections.Generic;
using System.Linq;
using NssApp.RestApi;
using NssRestClient;
using NssRestClient.Services;
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

            var result = await new SystemService(new RestClient(new NssHttpClientFactory(), new ClientCredentialStore())).GetLoggedInUser();
            if (result.LoginRequired == false)
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