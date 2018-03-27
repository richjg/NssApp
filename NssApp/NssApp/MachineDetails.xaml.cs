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
	public partial class MachineDetails : ContentPage
	{
		public MachineDetails(Machine machine)
		{
			InitializeComponent();
            this.BindingContext = new MachineDetailsViewModel(new NssRestApiService(new UserCredentialStore(), new HttpClientFactory())).Initialize(this, machine);
		}
	}
}