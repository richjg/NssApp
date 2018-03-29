using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

        public class Tile
        {
            public string Title { get; set; }
            public string Color { get; set; }
            public string Text { get; set; }
        }

        public ICommand ShowProtectionOptionsCommand { get => new Command(async () => await ShowProtectionOptions()); }

        private async void PageOnAppearing(object sender, EventArgs e)
        {
            this.MachineProtection = await this.nssRestApiService.GetMachineProtection(this.Machine.Id).ResolveData(this._CurrentPage);
            this.HasProtectionLevels = this.MachineProtection.ProtectedLevels.Any();
            this.AvailableProtectionLevels = await this.nssRestApiService.GetAvailableMachineProtectionLevels(this.Machine.Id).ResolveData(this._CurrentPage);
            this.ProtectionStatus = this.GetProtectionStatus(this.MachineProtection);
            this.LastSuccessfulBackupStatus = this.GetLastSuccessfulBackupStatus(await this.nssRestApiService.GetMachineImages(this.Machine.Id).ResolveData(this._CurrentPage));
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

        private async Task ShowProtectionOptions()
        {
            if(this.AvailableProtectionLevels != null)
            {
                var levelNames= this.AvailableProtectionLevels.Select(p => p.Name).ToArray();
                var selected = await this._CurrentPage.DisplayActionSheet("Protect", "Close", null, levelNames);
            }
        }
    }
}
