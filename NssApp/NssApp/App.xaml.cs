using NssApp.Navigation;
using NssApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NssApp
{
    public partial class App : Application
    {
        static App()
        {
            BuildDependencies();
        }

        public App()
        {
            InitializeComponent();
            InitNavigation();
        }

        public static void BuildDependencies()
        {

            Locator.Instance.Build();
        }

        private Task InitNavigation()
        {
            var navigationService = Locator.Instance.Resolve<INavigationService>();
            return navigationService.NavigateToAsync<SplashScreenViewModel>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
        }

        public static Func<Task> GetLoginFailedMethod(Page p)
        {
            return () => p.DisplayAlert("Connecting", "Hmm having trouble connecting to the server. Goto setting's to take a look", "Ok");
        }

        public static Func<Task> GetHttpErrorMethod(Page p)
        {
            return () => p.DisplayAlert("Connecting", "Hmm having trouble connecting to the server.", "Ok");
        }
    }
}