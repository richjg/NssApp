using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await LoadCounts();
                });
            }
        }

        protected override async void OnAppearing()
        {
            try
            {
                lv.BindingContext = this;
                await this.LoadCounts();
            }
            catch (Exception e)
            {
                Exception ee = e;
            }

            base.OnAppearing();
        }

        private async Task LoadCounts()
        {
            this.IsRefreshing = true;
            try
            {
                var trafficLightCounts = await NssRestClient.Instance.GetTrafficLightCounts().ResolveData(this);
                if (trafficLightCounts != null)
                {
                    RedCount.Text = trafficLightCounts.RedCount;
                    AmberCount.Text = trafficLightCounts.AmberCount;
                    GreenCount.Text = trafficLightCounts.GreenCount;
                }
            }
            finally
            {
                this.IsRefreshing = false;
            }
        }
    }
}