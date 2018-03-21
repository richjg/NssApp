using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NssApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Machines : ContentPage
    {
        public Machines()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                machineList.ItemsSource = await App.NssRestApi.GetComputers();
            }
            catch (Exception e)
            {
                Exception ee = e;
            }

            base.OnAppearing();
        }
    }
}