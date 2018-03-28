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

        private Machine _machine;
        public Machine Machine { get => this._machine; set => this.SetPropertyValue(ref _machine, value, nameof(Machine)); }

        private MachineProtection _machineProtection;
        public MachineProtection MachineProtection { get => this._machineProtection; set => this.SetPropertyValue(ref _machineProtection, value, nameof(MachineProtection)); }

        private List<ApiProtectionLevel> _availableProtectionLevels;
        public List<ApiProtectionLevel> AvailableProtectionLevels { get => this._availableProtectionLevels; set => this.SetPropertyValue(ref _availableProtectionLevels, value, nameof(AvailableProtectionLevels)); }



        public ICommand ShowProtectionOptionsCommand { get => new Command(async () => await ShowProtectionOptions()); }

        private async void PageOnAppearing(object sender, EventArgs e)
        {
            this.MachineProtection = await this.nssRestApiService.GetMachineProtection(this.Machine.Id).ResolveData(this._CurrentPage);
            this.AvailableProtectionLevels = await this.nssRestApiService.GetAvailableMachineProtectionLevels(this.Machine.Id).ResolveData(this._CurrentPage);

            //this.MachineProtection.ProtectedLevels[0].ProtectionLevel.Color.Policies[0].
        }

        private async Task ShowProtectionOptions()
        {
//            var selected = await this._CurrentPage.DisplayActionSheet("Protect", "Close", null, new[] { "Gold", "Silver", "Bronze" });
            if(this.AvailableProtectionLevels != null)
            {
                var levelNames= this.AvailableProtectionLevels.Select(p => p.Name).ToArray();
                var selected = await this._CurrentPage.DisplayActionSheet("Protect", "Close", null, levelNames);
            }
        }
    }
}
