using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
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

        private void PageOnAppearing(object sender, EventArgs e)
        {

        }
    }
}
