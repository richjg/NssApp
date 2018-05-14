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
	public partial class More : ContentPage
	{
		public More()
		{
			InitializeComponent();
            this.BindingContext = new MoreViewModel(new LoggedInUserService(new NssRestApiService(new UserCredentialStore(), new HttpClientFactory()), new UserCredentialStore())).Initialize(this);
        }
	}
}