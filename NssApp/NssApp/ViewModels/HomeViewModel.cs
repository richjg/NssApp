using ByteSizeLib;
using NssApp.RestApi;
using NssRestClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using NssApp.ViewModels;

namespace NssApp.ViewModels
{
    public class HomeViewModel : ViewModelBase, IHandleViewAppearing, IHandleViewDisappearing
    {
        private readonly IDashboardService dashboardService;
        private readonly ISystemService systemService;
        private readonly IUtilizationService utilizationService;

        public HomeViewModel(IDashboardService dashboardService, ISystemService systemService, IUtilizationService utilizationService)
        {
            this.dashboardService = dashboardService;
            this.systemService = systemService;
            this.utilizationService = utilizationService;
        }

        private bool _isUserMsp;
        public bool IsUserMsp { get => this._isUserMsp; set { this._isUserMsp = value; this.OnPropertyChanged(); } }

        private Tile _attentionTile;
        public Tile AttentionTile { get => this._attentionTile; set { this._attentionTile = value; this.OnPropertyChanged(); } }

        private Tile _unprotectedTile;
        public Tile UnprotectedTile { get => this._unprotectedTile; set { this._unprotectedTile = value; this.OnPropertyChanged(); } }

        private Tile _protectedTile;
        public Tile ProtectedTile { get => this._protectedTile; set { this._protectedTile = value; this.OnPropertyChanged(); } }

        private Tile _consumedCapacityTile;
        public Tile ConsumedCapacityTile { get => this._consumedCapacityTile; set { this._consumedCapacityTile = value; this.OnPropertyChanged(); } }

        private List<DataSourceItem> _chartData;
        public List<DataSourceItem> ChartData { get => this._chartData; set { this._chartData = value; this.OnPropertyChanged(); } }

        public override async Task InitializeAsync(object navigationData)
        {
            await this.LoadData();
        }

        public Task OnViewAppearingAsync(VisualElement view)
        {
            return Task.FromResult(true);
        }

        public Task OnViewDisappearingAsync(VisualElement view)
        {
            return Task.FromResult(true);
        }

        private async Task LoadData()
        {
            this.IsBusy = true;
            try
            {
                var trafficLightCounts = await dashboardService.GetTrafficLightForSystem().ResolveData();
                if (trafficLightCounts != null)
                {
                    AttentionTile = new Tile { Color = "#ea683c", Title = "Attention", Text = GetTrafficLightValue(trafficLightCounts.RedCount) };
                    UnprotectedTile = new Tile { Color = "#fcb53e", Title = "Unprotected", Text = GetTrafficLightValue(trafficLightCounts.AmberCount) };
                    ProtectedTile = new Tile { Color = "#bdcc2a", Title = "Protected", Text = GetTrafficLightValue(trafficLightCounts.GreenCount) };
                }

                IsUserMsp = (await systemService.GetLoggedInUser().ResolveData())?.IsMsp ?? false;

                if (IsUserMsp)
                {
                    var systemUtilisationMonths = await utilizationService.GetSystemUtilisationMonths().ResolveData();

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
                this.IsBusy = false;
            }

            string GetTrafficLightValue(int count)
            {
                if (count > 9999)
                {
                    return (count / 1000).ToString("N0") + "k";
                }
                else
                {
                    return count.ToString("N0");
                }
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
