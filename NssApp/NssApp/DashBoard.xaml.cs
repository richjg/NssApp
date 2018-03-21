﻿using System;
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
                var trafficLightCounts = await App.NssRestApi.GetTrafficLightCounts();

                RedCount.Text = trafficLightCounts.RedCount;
                AmberCount.Text = trafficLightCounts.AmberCount;
                GreenCount.Text = trafficLightCounts.GreenCount;
            }
            catch (Exception e)
            {
                Exception ee = e;
            }

            base.OnAppearing();
        }
    }
}