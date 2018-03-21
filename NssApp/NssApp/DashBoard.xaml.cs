using NssApp.RestApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NssApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DashBoard : ContentPage
	{
		public DashBoard ()
		{
			InitializeComponent ();

            
		}


        protected override async void OnAppearing()
        {
            try
            {
                var machines = await App.NssRestApi.GetComputers();
                this.machineList.ItemsSource = machines;
            }
            catch (Exception e)
            {
                Exception ee = e;
            }
            base.OnAppearing();
        }

        //private async void LoadData()
        //{
        //    try
        //    {
        //        var machines = App.NssRestApi.GetComputers().GetAwaiter().GetResult();
        //        this.machineList.ItemsSource = machines;
        //    }catch(Exception e)
        //    {
        //        Exception ee = e;
        //    }
        //}
	}

}