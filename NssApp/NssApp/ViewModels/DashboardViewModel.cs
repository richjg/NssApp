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
    //public class DashboardViewModel : INotifyPropertyChanged
    //{
    //    public ObservableCollection<CategoricalData> Data { get; set; }

    //    private static ObservableCollection<CategoricalData> GetCategoricalData()
    //    {
    //        var data = new ObservableCollection<CategoricalData>  {
    //        new CategoricalData { Category = "A", Value = 0.63 },
    //        new CategoricalData { Category = "B", Value = 0.85 },
    //        new CategoricalData { Category = "C", Value = 1.05 },
    //        new CategoricalData { Category = "D", Value = 0.96 },
    //        new CategoricalData { Category = "E", Value = 0.78 },
    //    };

    //        return data;
    //    }

    //    private bool _isRefreshing = false;

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    private void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    //    public bool IsRefreshing
    //    {
    //        get { return _isRefreshing; }
    //        set
    //        {
    //            _isRefreshing = value;
    //            OnPropertyChanged(nameof(IsRefreshing));
    //        }
    //    }

    //    public ICommand RefreshCommand
    //    {
    //        get
    //        {
    //            return new Command(async () =>
    //            {
    //                await LoadCounts();
    //            });
    //        }
    //    }

    //    public string RedCount { }


    //    private async Task LoadCounts()
    //    {
    //        this.IsRefreshing = true;
    //        try
    //        {
    //            var trafficLightCounts = await NssRestClient.Instance.GetTrafficLightCounts().ResolveData(App.Current.MainPage);
    //            if (trafficLightCounts != null)
    //            {
    //                RedCount.Text = trafficLightCounts.RedCount;
    //                AmberCount.Text = trafficLightCounts.AmberCount;
    //                GreenCount.Text = trafficLightCounts.GreenCount;
    //            }
    //        }
    //        finally
    //        {
    //            this.IsRefreshing = false;
    //        }
    //    }


    //    public class CategoricalData
    //    {
    //        public object Category { get; set; }

    //        public double Value { get; set; }
    //    }
    //}
}
