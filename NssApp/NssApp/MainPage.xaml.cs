using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NssApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        public async void LoginClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DashBoard());
            //new NavigationPage(new DashBoard())
        }
	}
}
