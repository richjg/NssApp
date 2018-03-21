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
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
		}
       
        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var isValid = await App.NssRestApi.Login(UsernameEntry.Text, PasswordEntry.Text);
            if (isValid)
            {
                App.IsUserLoggedIn = true;
                Navigation.InsertPageBefore(new DashBoard(), this);
                await Navigation.PopAsync();
            }
            else
            {
                MessageLabel.Text = "Login failed";
                PasswordEntry.Text = string.Empty;
            }
        }
    }
}