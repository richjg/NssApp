using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NssApp.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Page _CurrentPage;

        public DashboardViewModel Initialize(Page page)
        {
            this._CurrentPage = page;
            page.Appearing += this.PageOnAppearing;
            return this;
        }

        private void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void SetPropertyValue<T>(ref T t, T value, string name)
        {
            t = value;
            OnPropertyChanged(name);
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing { get => this._isRefreshing; set => this.SetPropertyValue(ref _isRefreshing, value, nameof(IsRefreshing)); }

        private string _redCount;
        public string RedCount { get => this._redCount; set => SetPropertyValue(ref _redCount, value, nameof(RedCount)); }

        private string _amberCount;
        public string AmberCount { get => this._amberCount; set => SetPropertyValue(ref _amberCount, value, nameof(AmberCount)); }

        private string _greenCount;
        public string GreenCount { get => this._greenCount; set => SetPropertyValue(ref _greenCount, value, nameof(GreenCount)); }

        public ICommand PullToRefreshCommand
        {
            get
            {
                return new Command(async () => 
                {
                    await LoadCounts();
                });
            }
        }

        private async void PageOnAppearing(object sender, EventArgs e)
        {
            await LoadCounts();
        }

        private async Task LoadCounts()
        {
            this.IsRefreshing = true;
            try
            {
                var trafficLightCounts = await NssRestClient.Instance.GetTrafficLightCounts().ResolveData(this._CurrentPage/*App.Current.MainPage*/);
                if (trafficLightCounts != null)
                {
                    RedCount = trafficLightCounts.RedCount;
                    AmberCount = trafficLightCounts.AmberCount;
                    GreenCount = trafficLightCounts.GreenCount;
                }
            }
            finally
            {
                this.IsRefreshing = false;
            }
        }
    }
}
