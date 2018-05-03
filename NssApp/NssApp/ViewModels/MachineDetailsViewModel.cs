using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ByteSizeLib;
using Xamarin.Forms;

namespace NssApp.ViewModels
{
    public class MachineDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Page _CurrentPage;
        private INavigation _Navigation;
        private readonly NssRestApiService nssRestApiService;

        public MachineDetailsViewModel(NssRestApiService nssRestApiService)
        {
            this.nssRestApiService = nssRestApiService;
        }

        public MachineDetailsViewModel Initialize(Page page, Machine machine)
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

        private Machine _machine;
        public Machine Machine { get => this._machine; set => this.SetPropertyValue(ref _machine, value, nameof(Machine)); }

        private MachineProtection _machineProtection;
        public MachineProtection MachineProtection { get => this._machineProtection; set => this.SetPropertyValue(ref _machineProtection, value, nameof(MachineProtection)); }

        private List<ApiProtectionLevel> _availableProtectionLevels;
        public List<ApiProtectionLevel> AvailableProtectionLevels { get => this._availableProtectionLevels; set => this.SetPropertyValue(ref _availableProtectionLevels, value, nameof(AvailableProtectionLevels)); }

        private Tile _protectionStatus;
        public Tile ProtectionStatus { get => this._protectionStatus; set => this.SetPropertyValue(ref _protectionStatus, value, nameof(ProtectionStatus)); }

        private Tile _lastSuccessfulBackupStatus;
        public Tile LastSuccessfulBackupStatus { get => this._lastSuccessfulBackupStatus; set => this.SetPropertyValue(ref _lastSuccessfulBackupStatus, value, nameof(LastSuccessfulBackupStatus)); }

        private Tile _consumedCapacity;
        public Tile ConsumedCapacity { get => this._consumedCapacity; set => this.SetPropertyValue(ref _consumedCapacity, value, nameof(ConsumedCapacity)); }

        public ICommand ShowProtectionOptionsCommand { get => new Command(async () => await ShowProtectionOptions()); }

        private async void PageOnAppearing(object sender, EventArgs e)
        {
            this.MachineProtection = await this.nssRestApiService.GetMachineProtection(this.Machine.Id).ResolveData(this._CurrentPage);
            this.HasProtectionLevels = this.MachineProtection.ProtectedLevels.Any();
            this.AvailableProtectionLevels = await this.nssRestApiService.GetAvailableMachineProtectionLevels(this.Machine.Id).ResolveData(this._CurrentPage);
            this.ProtectionStatus = this.GetProtectionStatus(this.MachineProtection);
            this.LastSuccessfulBackupStatus = this.GetLastSuccessfulBackupStatus(await this.nssRestApiService.GetMachineImages(this.Machine.Id).ResolveData(this._CurrentPage));

            var loggedInUser = await this.nssRestApiService.GetCurrentUserInfo();
            if (loggedInUser.IsMsp || loggedInUser.IsTenantAdmin)
            {
                this.ConsumedCapacity = this.GetConsumedCapacity(await this.nssRestApiService.GetMachineUtilisationMonths(this.Machine.Id).ResolveData(this._CurrentPage));
            }
            else
            {
                this.ConsumedCapacity = this.GetConsumedCapacity(new List<MachineUtilisationMonth>());
            }
        }

        private Tile GetProtectionStatus(MachineProtection machineProtection)
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

        private Tile GetLastSuccessfulBackupStatus(List<MachineImage> machineImages)
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

        private Tile GetConsumedCapacity(List<MachineUtilisationMonth> machineUtilisationMonths)
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
                        var activity = await this.nssRestApiService.ProtectMachine(this.Machine.Id, selectedLevel.Id).ResolveData(this._CurrentPage); // not showing errors, what errors would there be?
                        if(activity != null)
                        {

                        }
                    }
                }
            }
        }
    }
}
