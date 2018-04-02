using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using NssApp.RestApi;
using NssApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NssApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Machines : ContentPage
    {
        public Machines()
        {
            InitializeComponent();
            this.BindingContext = new MachineViewModel(new NssRestApiService(new UserCredentialStore(), new HttpClientFactory())).Initialize(this);
        }
    }
}