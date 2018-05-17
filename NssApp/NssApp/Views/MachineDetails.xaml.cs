using NssApp.RestApi;
using NssApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NssRestClient;
using NssRestClient.Dto;
using NssRestClient.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NssApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MachineDetails : ContentPage
	{
		public MachineDetails(ApiMachine machine)
		{
			InitializeComponent();
            this.BindingContext = new MachineDetailsViewModel(new MachineService(new RestClient(new NssHttpClientFactory(), new ClientCredentialStore())), new SystemService(new RestClient(new NssHttpClientFactory(), new ClientCredentialStore()))).Initialize(this, machine);
		}
	}
}