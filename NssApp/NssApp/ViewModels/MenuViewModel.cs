using NssRestClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NssApp.ViewModels
{
    public class MenuViewModel : ViewModelBase, IHandleViewAppearing, IHandleViewDisappearing
    {
        private ObservableCollection<Models.MenuItem> _menuItems;
        private readonly ILoginService _loginService;

        public MenuViewModel(ILoginService loginService)
        {
            MenuItems = new ObservableCollection<Models.MenuItem>();
            this._loginService = loginService;
            InitMenuItems();
        }

        private string _username;
        public string Username { get => this._username; set { this._username = value; this.OnPropertyChanged(); } }

        public string UserAvatar => "Todo";

        public ObservableCollection<Models.MenuItem> MenuItems
        {
            get
            {
                return _menuItems;
            }
            set
            {
                _menuItems = value;
                OnPropertyChanged();
            }
        }

        public ICommand MenuItemSelectedCommand => new Command<Models.MenuItem>(OnSelectMenuItem);

        public async override Task InitializeAsync(object navigationData)
        {
            var loginSettings = await this._loginService.GetCurrentLoginSettings();
            this.Username = loginSettings?.Username ?? "";
        }

        public Task OnViewAppearingAsync(VisualElement view)
        {
            //MessagingCenter.Subscribe<Booking>(this, MessengerKeys.BookingRequested, OnBookingRequested);
            //MessagingCenter.Subscribe<CheckoutViewModel>(this, MessengerKeys.CheckoutRequested, OnCheckoutRequested);
            return Task.CompletedTask;
        }

        public Task OnViewDisappearingAsync(VisualElement view)
        {
            return Task.FromResult(true);
        }

        private void InitMenuItems()
        {
            MenuItems.Add(new Models.MenuItem
            {
                Title = "Home",
                ViewModelType = typeof(MainViewModel),
                IsEnabled = true
            });

            MenuItems.Add(new Models.MenuItem
            {
                Title = "Computers",
                ViewModelType = typeof(ComputerListViewModel),
                IsEnabled = true
            });

            MenuItems.Add(new Models.MenuItem
            {
                Title = "Logout",
                ViewModelType = typeof(LoginViewModel),
                IsEnabled = true,
                AfterNavigationAction = RemoveUserCredentials
            });
        }

        private async void OnSelectMenuItem(Models.MenuItem item)
        {
            if (item.IsEnabled && item.ViewModelType != null)
            {
                item.AfterNavigationAction?.Invoke();
                await NavigationService.NavigateToAsync(item.ViewModelType, item);
            }
        }

        private Task RemoveUserCredentials()
        {
            return this._loginService.SignOutAsync();
        }
    }
}
