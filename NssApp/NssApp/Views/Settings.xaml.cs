using NssApp.RestApi;
using NssApp.ViewModels;
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
            this.BindingContext = new SettingsViewModel(new LoggedInUserService(new NssRestApiService(new UserCredentialStore(), new HttpClientFactory()), new UserCredentialStore())).Initialize(this);
		}
    }
}