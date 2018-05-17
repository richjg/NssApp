using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ByteSizeLib;
using NssRestClient.Dto;
using NssRestClient.Services;
using Xamarin.Forms;
using ApiProtectionLevel = NssRestClient.Dto.ApiProtectionLevel;

namespace NssApp.ViewModels
{
    public class MachineDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Page _CurrentPage;
        private INavigation _Navigation;
        private readonly MachineService machineService;
        private readonly SystemService systemService;

        public MachineDetailsViewModel(MachineService machineService, SystemService systemService)
        {
            this.machineService = machineService;
            this.systemService = systemService;
        }

        public MachineDetailsViewModel Initialize(Page page, ApiMachine machine)
        {
            this._machine = machine;
            this._CurrentPage = page;
            this._Navigation = page.Navigation;
            page.Appearing += this.PageOnAppearing;
            return this;
        }

        private void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void SetPropertyValue<T>(ref T t, T value, string name)
        {
            t = value;
            OnPropertyChanged(name);
        }
        
        private bool _hasProtectionLevels;
        public bool HasProtectionLevels { get => this._hasProtectionLevels; set => this.SetPropertyValue(ref _hasProtectionLevels, value, nameof(HasProtectionLevels)); }

        private ApiMachine _machine;
        public ApiMachine Machine { get => this._machine; set => this.SetPropertyValue(ref _machine, value, nameof(Machine)); }

        private ApiProtected _machineProtection;
        public ApiProtected MachineProtection { get => this._machineProtection; set => this.SetPropertyValue(ref _machineProtection, value, nameof(MachineProtection)); }

        private List<NssRestClient.Dto.ApiProtectionLevel> _availableProtectionLevels;
        public List<ApiProtectionLevel> AvailableProtectionLevels { get => this._availableProtectionLevels; set => this.SetPropertyValue(ref _availableProtectionLevels, value, nameof(AvailableProtectionLevels)); }

        private Tile _protectionStatus;
        public Tile ProtectionStatus { get => this._protectionStatus; set => this.SetPropertyValue(ref _protectionStatus, value, nameof(ProtectionStatus)); }

        private Tile _lastSuccessfulBackupStatus;
        public Tile LastSuccessfulBackupStatus { get => this._lastSuccessfulBackupStatus; set => this.SetPropertyValue(ref _lastSuccessfulBackupStatus, value, nameof(LastSuccessfulBackupStatus)); }

        private Tile _consumedCapacity;
        public Tile ConsumedCapacity { get => this._consumedCapacity; set => this.SetPropertyValue(ref _consumedCapacity, value, nameof(ConsumedCapacity)); }

        public ICommand ShowProtectionOptionsCommand { get => new Command(async () => await ShowProtectionOptions()); }

        public bool _isRefreshing;
        public bool IsRefreshing { get => this._isRefreshing; set => this.SetPropertyValue(ref _isRefreshing, value, nameof(IsRefreshing)); }
        
        public ICommand PullToRefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await LoadDetails();
                });
            }
        }

        private async void PageOnAppearing(object sender, EventArgs e) => await LoadDetails();

        private async Task LoadDetails()
        {
            try
            {
                this.IsRefreshing = true;


                this.MachineProtection = await this.machineService.GetMachineProtection(this.Machine.Id).ResolveData(this._CurrentPage);
                if (this.MachineProtection != null)
                {
                    this.HasProtectionLevels = this.MachineProtection.ProtectedLevels.Any();
                    this.AvailableProtectionLevels = await this.machineService.GetAvailableMachineProtectionLevels(this.Machine.Id).ResolveData(this._CurrentPage);
                    this.ProtectionStatus = this.GetProtectionStatus(this.MachineProtection);
                    var machineImages = await this.machineService.GetMachineImages(this.Machine.Id).ResolveData(this._CurrentPage);
                    if (machineImages != null)
                    {
                        this.LastSuccessfulBackupStatus = this.GetLastSuccessfulBackupStatus(machineImages);
                    }

                    var loggedInUser = await this.systemService.GetLoggedInUser().ResolveData(this._CurrentPage);
                    if (loggedInUser != null)
                    {
                        if (loggedInUser.IsMsp || loggedInUser.IsTenantAdmin)
                        {
                            this.ConsumedCapacity = this.GetConsumedCapacity(await this.machineService.GetMachineUtilisationMonths(this.Machine.Id).ResolveData(this._CurrentPage));
                        }
                        else
                        {
                            this.ConsumedCapacity = this.GetConsumedCapacity(new List<ApiMachineUtilisationMonth>());
                        }
                    }
                }
            }
            finally
            {
                this.IsRefreshing = false;
            }
        }

        private Tile GetProtectionStatus(ApiProtected machineProtection)
        {
            var tile = new Tile
            {
                Title = "Protection Status"
            };

            if (machineProtection.ProtectedLevels.Any() == false)
            {
                tile.Color = "#fcb53e";
                tile.Text = "Not protected";
            }
            else
            {
                if (machineProtection.ProtectedLevels.SelectMany(l => l.Policies).Any(mp => mp.IsWithinThreshold == false))
                {
                    tile.Color = "#ea683c";
                    tile.Text = "Some backups have exceeded threshold";
                }
                else
                {
                    tile.Color = "#bdcc2a";
                    tile.Text = "Backup health is good";
                }
            }

            return tile;
        }

        private Tile GetLastSuccessfulBackupStatus(List<ApiBackupImage> machineImages)
        {
            var tile = new Tile
            {
                Title = "Last Backup"
            };

            if (machineImages.Any(i => i.IsExpired == false))
            {
                tile.Color = "#bdcc2a";
                tile.Text = machineImages.OrderByDescending(i => i.BackupTime).First().BackupTime.ToShortDateString();
            }
            else
            {
                tile.Color = "#fcb53e";
                tile.Text = "No backups found for this computer";
            }

            return tile;
        }

        private Tile GetConsumedCapacity(List<ApiMachineUtilisationMonth> machineUtilisationMonths)
        {
            var tile = new Tile
            {
                Color = "#76b0bd",
                Title = "Consumed Capacity",
                Text = "0"
            };

            var latest = machineUtilisationMonths.OrderByDescending(m => m.Date).FirstOrDefault();
            if (latest != null)
            {
                //TODO: Decide if we should use EndTotalImageSizeBytes or EndTotalTransferredSizeBytes based on integration setting 'Use Data Transferred values'
                tile.Text = $"{ByteSize.FromBytes(latest.EndTotalImageSizeBytes).LargestWholeNumberValue:0} {ByteSize.FromBytes(latest.EndTotalImageSizeBytes).LargestWholeNumberSymbol}";
            }

            return tile;
        }

        private async Task ShowProtectionOptions()
        {
            if(this.AvailableProtectionLevels != null)
            {
                //Think we need a proper modal page here as we need to know if its .. this._Navigation.PushModalAsync
                var levels = this.AvailableProtectionLevels.ToList();
                var levelNames = this.AvailableProtectionLevels.Select(p => p.Name).ToArray();
                var selectedLevelName = await this._CurrentPage.DisplayActionSheet("Protect", "Close", null, levelNames);
                if (String.IsNullOrWhiteSpace(selectedLevelName) == false)
                {
                    var selectedLevel = levels.FirstOrDefault(l => l.Name == selectedLevelName);
                    if(selectedLevel != null && selectedLevel.IsBackupNow == false)
                    {
                        await this.machineService.ProtectMachine(this.Machine.Id, selectedLevel.Id).ResolveData(this._CurrentPage); // not showing errors, what errors would there be?
                    }
                }
            }
        }
    }
}
