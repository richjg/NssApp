using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using NssRestClient.Dto;
using NssRestClient.Services;
using Xamarin.Forms;

namespace NssApp.ViewModels
{
    public class MachineViewModel : INotifyPropertyChanged
    {
        private readonly MachineService machineService;
        public event PropertyChangedEventHandler PropertyChanged;
        private Page _CurrentPage;
        private INavigation _Navigation;

        public MachineViewModel(MachineService machineService)
        {
            this.machineService = machineService;
        }

        public MachineViewModel Initialize(Page page)
        {
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
        
        private bool _isRefreshing = false;
        public bool IsRefreshing { get => this._isRefreshing; set => this.SetPropertyValue(ref _isRefreshing, value, nameof(IsRefreshing)); }

        private int CurrrentPageIndex { get; set; } = 1;
        private bool HasMoreItems { get; set; } = true;

        public ObservableCollection<ApiMachine> MachineCollection { get; set; } = new ObservableCollection<ApiMachine>();
        public ICommand LoadMoreCommand { get => new Command<ApiMachine>(async (m) => await this.LoadMachines(), this.CanLoadMore); }
        public ICommand MachineSelectedCommand { get => new Command<ApiMachine>(async (m) => await this.MachineSelected(m)); }

        private string _searchText;
        public string SearchText
        {
            get => this._searchText;
            set
            {
                try
                {
                    this.SetPropertyValue(ref _searchText, value, nameof(SearchText));
                    this.ResetLoadMore();
                }
                catch (Exception e)
                {
                    Exception ee = e;
                }
            }
        }

        private async void PageOnAppearing(object sender, EventArgs e)
        {
            await LoadMachines();
        }

        private async Task MachineSelected(ApiMachine machine)
        {
            await this._Navigation.PushAsync(new MachineDetails(machine));
        }

        private async void ResetLoadMore()
        {
            this.CurrrentPageIndex = 1;
            await LoadMachines();
        }

        private async Task LoadMachines()
        {
            this.IsRefreshing = true;

            try
            {
                if (this.CurrrentPageIndex == 1)
                {
                    this.MachineCollection.Clear();
                }

                var machines = await this.machineService.GetComputers(this.CurrrentPageIndex, 50, this.SearchText).ResolveData(this._CurrentPage);
                if (machines != null && machines.Any())
                {
                    this.CurrrentPageIndex++;
                    foreach (var machine in machines)
                    {
                        MachineCollection.Add(machine);
                    }

                    HasMoreItems = true;
                }
                else
                {
                    HasMoreItems = false;
                }
            }
            catch (Exception e)
            {
                Exception ee = e;
            }
            finally
            {
                this.IsRefreshing = false;
            }
        }

        private bool CanLoadMore(ApiMachine machine)
        {
            return IsRefreshing == false && HasMoreItems && MachineCollection.Count != 0 && MachineCollection.Last() == machine;
        }
    }
}
