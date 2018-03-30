﻿using NssApp.RestApi;
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
                    AttentionTile = new Tile { Color = "#ea683c", Title = "Attention", Text = trafficLightCounts.RedCount };
                    UnprotectedTile = new Tile { Color = "#fcb53e", Title = "Unprotected", Text = trafficLightCounts.AmberCount };
                    ProtectedTile = new Tile { Color = "#bdcc2a", Title = "Protected", Text = trafficLightCounts.GreenCount };
                }

                IsUserMsp = (await nssRestApiService.GetCurrentUserInfo()).IsMsp;

                if (IsUserMsp)
                {
                    var tile = new Tile
                    {
                        Color = "#76b0bd",
                        Title = "Consumed Capacity",
                        Text = "0"
                    };
                    var latest = (await nssRestApiService.GetSystemUtilisationMonths().ResolveData(this._CurrentPage)).OrderByDescending(m => m.Date).FirstOrDefault();
                    if (latest != null)
                    {
                        //TODO: Decide if we should use EndTotalImageSizeBytes or EndTotalTransferredSizeBytes based on integration setting 'Use Data Transferred values'
                        tile.Text = $"{ByteSize.FromBytes(latest.EndTotalTransferredSizeBytes).LargestWholeNumberValue:0} {ByteSize.FromBytes(latest.EndTotalTransferredSizeBytes).LargestWholeNumberSymbol}";
                    }

                    ConsumedCapacityTile = tile;
                }
            }
            finally
            {
                this.IsRefreshing = false;
            }
        }
    }
}
