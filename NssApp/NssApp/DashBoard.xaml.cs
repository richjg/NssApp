using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NssApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashBoard : ContentPage
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                var machines = await App.NssRestApi.GetComputers();
                machineList.ItemsSource = machines;
            }
            catch (Exception e)
            {
                Exception ee = e;
            }

            base.OnAppearing();
        }
    }
}