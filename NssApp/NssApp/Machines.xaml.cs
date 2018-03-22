using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using NssApp.RestApi;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NssApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Machines : ContentPage
    {
        public ObservableCollection<Machine> MachineCollection { get; set; } = new ObservableCollection<Machine>();
        public int CurrentPage { get; private set; } = 1;
        public bool HasMoreItems { get; set; } = true;

        public Machines()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                machineList.ItemAppearing += MachineListOnItemAppearing;
                machineList.ItemsSource = MachineCollection;
                await LoadMachines(CurrentPage);
            }
            catch (Exception e)
            {
                Exception ee = e;
            }

            base.OnAppearing();
        }

        protected async void MachineListOnItemAppearing(object sender, ItemVisibilityEventArgs itemVisibilityEventArgs)
        {
            if (sender is ListView listView && listView.ItemsSource is IList<Machine> items && itemVisibilityEventArgs.Item == items[items.Count - 1])
            {
                CurrentPage++;
                await LoadMachines(CurrentPage);
            }
        }

        private async Task LoadMachines(int page)
        {
            if (HasMoreItems == false)
                return;

            var machines = await App.NssRestApi.GetComputers(page, 20);
            HasMoreItems = machines.Count == 20;

            foreach (var machine in machines)
            {
                MachineCollection.Add(machine);
            }
        }
    }
}