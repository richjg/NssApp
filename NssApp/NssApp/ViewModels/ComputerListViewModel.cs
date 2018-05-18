using NssApp.RestApi;
using NssRestClient.Dto;
using NssRestClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NssApp.ViewModels
{
    public class ComputerListViewModel : ViewModelBase
    {
        private readonly IMachineService machineService;

        public ComputerListViewModel(IMachineService machineService)
        {
            this.machineService = machineService;
        }

        private int CurrrentPageIndex { get; set; } = 1;
        private bool HasMoreItems { get; set; } = true;
        public ObservableCollection<ApiMachine> MachineCollection { get; set; } = new ObservableCollection<ApiMachine>();

        public ICommand LoadMoreCommand => new AsyncCommand<ApiMachine>(this.LoadMachines, this.CanLoadMore);
        public ICommand MachineSelectedCommand => new AsyncCommand<ApiMachine>(this.MachineSelected);

        private string _searchText;
        public string SearchText
        {
            get => this._searchText;
            set
            {
                try
                {
                    this._searchText = value;
                    this.OnPropertyChanged();
                    this.ResetLoadMore();
                }
                catch (Exception e)
                {
                    Exception ee = e;
                }
            }
        }

        public async override Task InitializeAsync(object navigationData)
        {
            await this.LoadMachines();
        }
        private bool CanLoadMore(ApiMachine machine)
        {
            return this.IsBusy == false && HasMoreItems && MachineCollection.Count != 0 && MachineCollection.Last() == machine;
        }

        private async void ResetLoadMore()
        {
            this.CurrrentPageIndex = 1;
            await LoadMachines();
        }

        private async Task LoadMachines()
        {
            try
            {
                this.IsBusy = true;
                if (this.CurrrentPageIndex == 1)
                {
                    this.MachineCollection.Clear();
                }

                var machines = await this.machineService.GetComputers(this.CurrrentPageIndex, 50, this.SearchText).ResolveData();
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
                this.IsBusy = false;
            }
        }

        private async Task MachineSelected(ApiMachine machine)
        {
            await Task.CompletedTask;
            //await this._Navigation.PushAsync(new MachineDetails(machine));
        }

    }
}
