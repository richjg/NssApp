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
        public string SearchText { get; set; } = string.Empty;

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
                await LoadMachines(CurrentPage, SearchText);
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
                if (HasMoreItems)
                {
                    CurrentPage++;
                    await LoadMachines(CurrentPage, SearchText);
                }
            }
        }

        private async void Entry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            await LoadMachines(CurrentPage, e.NewTextValue);
        }

        private async Task LoadMachines(int page, string searchText)
        {
            if (SearchText != searchText)
            {
                SearchText = searchText;
                CurrentPage = 1;
                MachineCollection.Clear();
                HasMoreItems = true;
            }
            //Application.Current.SavePropertiesAsync

            var machines2 = await NssRestClient.Instance.GetComputers(page, 20, searchText).Match(valid: r => r, errors: (e) => { }, loginRequired: () => { });
            HasMoreItems = machines2.Count == 20;

            foreach (var machine in machines2)
            {
                MachineCollection.Add(machine);
            }
        }
        
        private void MachineList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Machine machine)
            {
                Navigation.PushAsync(new MachineDetails(machine.Id));
            }
        }
    }
}