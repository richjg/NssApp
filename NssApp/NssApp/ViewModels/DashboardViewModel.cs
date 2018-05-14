using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ByteSizeLib;
using Xamarin.Forms;

namespace NssApp.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Page _CurrentPage;

        public DashboardViewModel(NssRestApiService nssRestApiService)
        {
            this.nssRestApiService = nssRestApiService;
        }

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

        private bool _isUserMsp;
        public bool IsUserMsp { get => this._isUserMsp; set => this.SetPropertyValue(ref _isUserMsp, value, nameof(IsUserMsp)); }

        private Tile _attentionTile;
        public Tile AttentionTile { get => this._attentionTile; set => SetPropertyValue(ref _attentionTile, value, nameof(AttentionTile)); }

        private Tile _unprotectedTile;
        public Tile UnprotectedTile { get => this._unprotectedTile; set => SetPropertyValue(ref _unprotectedTile, value, nameof(UnprotectedTile)); }

        private Tile _protectedTile;
        public Tile ProtectedTile { get => this._protectedTile; set => SetPropertyValue(ref _protectedTile, value, nameof(ProtectedTile)); }

        private Tile _consumedCapacityTile;
        public Tile ConsumedCapacityTile { get => this._consumedCapacityTile; set => SetPropertyValue(ref _consumedCapacityTile, value, nameof(ConsumedCapacityTile)); }

        private List<DataSourceItem> _chartData;
        public List<DataSourceItem> ChartData { get => this._chartData; set => SetPropertyValue(ref _chartData, value, nameof(ChartData)); }

        private readonly NssRestApiService nssRestApiService;

        public ICommand PullToRefreshCommand
        {
            get
            {
                return new Command(async () => 
                {
                    await LoadTiles();
                });
            }
        }

        private async void PageOnAppearing(object sender, EventArgs e)
        {
            await LoadTiles();
        }

        private async Task LoadTiles()
        {
            this.IsRefreshing = true;
            try
            {
                var trafficLightCounts = await nssRestApiService.GetTrafficLightCounts().ResolveData(this._CurrentPage/*App.Current.MainPage*/);
                if (trafficLightCounts != null)
                {
                    AttentionTile = new Tile { Color = "#ea683c", Title = "Attention", Text = GetTrafficLightValue(trafficLightCounts.RedCount) };
                    UnprotectedTile = new Tile { Color = "#fcb53e", Title = "Unprotected", Text = GetTrafficLightValue(trafficLightCounts.AmberCount) };
                    ProtectedTile = new Tile { Color = "#bdcc2a", Title = "Protected", Text = GetTrafficLightValue(trafficLightCounts.GreenCount) };
                }

                IsUserMsp = (await nssRestApiService.GetCurrentUserInfo())?.IsMsp ?? false;

                if (IsUserMsp)
                {
                    var systemUtilisationMonths = await nssRestApiService.GetSystemUtilisationMonths().ResolveData(this._CurrentPage);

                    if (systemUtilisationMonths != null)
                    {

                        var tile = new Tile
                        {
                            Color = "#76b0bd",
                            Title = "Consumed Capacity",
                            Text = "0"
                        };
                        var latest = systemUtilisationMonths.OrderByDescending(m => m.Date).FirstOrDefault();
                        if (latest != null)
                        {
                            //TODO: Decide if we should use EndTotalImageSizeBytes or EndTotalTransferredSizeBytes based on integration setting 'Use Data Transferred values'
                            tile.Text = $"{ByteSize.FromBytes(latest.EndTotalTransferredSizeBytes).LargestWholeNumberValue:0} {ByteSize.FromBytes(latest.EndTotalTransferredSizeBytes).LargestWholeNumberSymbol}";
                        }

                        ConsumedCapacityTile = tile;

                        //TODO: Decide if we should use EndTotalImageSizeBytes or EndTotalTransferredSizeBytes based on integration setting 'Use Data Transferred values'
                        ChartData = systemUtilisationMonths.OrderByDescending(m => m.Date).Take(6).Select(m => new RestDataPoint { DateTime = m.Date, Value = m.NewTransferredSizeBytes }).ToList().AddMonthlyDataPointsAndOrder(6);
                    }
                }
            }
            finally
            {
                this.IsRefreshing = false;
            }
        }

        private string GetTrafficLightValue(int count)
        {
            if (count > 9999)
            {
                return  (count / 1000).ToString("N0") + "k";
            }
            else
            {
                return count.ToString("N0");
            }
        }

        public class RestDataPoint
        {
            public DateTime DateTime { get; set; }
            public long Value { get; set; }
        }

        public class DataSourceItem
        {
            public string Label { get; set; }
            public decimal Value { get; set; }
        }
    }
}
