using NssApp.RestApi;
using NssApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NssRestClient;
using NssRestClient.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NssApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashBoard : ContentPage
    {
        public DashBoard()
        {
            InitializeComponent();
            this.BindingContext = new DashboardViewModel(new DashboardService(new RestClient(new NssHttpClientFactory(), new ClientCredentialStore())), new SystemService(new RestClient(new NssHttpClientFactory(), new ClientCredentialStore())), new UtilizationService(new RestClient(new NssHttpClientFactory(), new ClientCredentialStore()))).Initialize(this);
        }
    }
}