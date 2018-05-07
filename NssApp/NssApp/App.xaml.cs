using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NssApp
{
	public partial class App : Application
	{
        public static LoggedInUserInfo LoggedInUserInfo { get; set; }

        public App ()
		{
			InitializeComponent();
            MainPage = new SplashScreen();
        }

		protected override void OnStart ()
		{
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
        }

        public static Func<Task> GetLoginFailedMethod(Page p)
        {
            return () => p.DisplayAlert("Connecting", "Hmm having trouble connecting to the server. Goto setting's to take a look", "Ok");
        }
	}
}
