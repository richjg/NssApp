using NssApp.Dialog;
using NssApp.Navigation;
using NssApp.RestApi;
using NssRestClient;
using NssRestClient.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace NssApp.ViewModels
{
    public class Locator
    {
        private IUnityContainer _container;

        public static readonly Locator Instance = new Locator();

        public Locator()
        {
            _container = new UnityContainer();


            _container.RegisterType<INavigationService, NavigationService>();
            _container.RegisterType<IDialogService, DialogService>();
            _container.RegisterType<IClientCredentialStore, ClientCredentialStore>();
            _container.RegisterType<INssHttpClientFactory, NssHttpClientFactory>();
            _container.RegisterType<IRestClient, RestClient>();
            _container.RegisterType<ILoginService, LoginService>();
            _container.RegisterType<IMachineService, MachineService>();
            _container.RegisterType<ISystemService, SystemService>();
            _container.RegisterType<IDashboardService, DashboardService>();
            _container.RegisterType<IUtilizationService, UtilizationService>();

            _container.RegisterType<ComputerListViewModel>();
            _container.RegisterType<HomeViewModel>();
            _container.RegisterType<LoginViewModel>();
            _container.RegisterType<MenuViewModel>();
            _container.RegisterType<MainViewModel>();

            //_containerBuilder.RegisterType<AnalyticService>().As<IAnalyticService>();
            //_containerBuilder.RegisterType<DialogService>().As<IDialogService>();
            //_containerBuilder.RegisterType<NavigationService>().As<INavigationService>();
            //_containerBuilder.RegisterType<FakeChartService>().As<IChartService>();
            //_containerBuilder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
            //_containerBuilder.RegisterType<LocationService>().As<ILocationService>();
            //_containerBuilder.RegisterType<OpenUriService>().As<IOpenUriService>();
            //_containerBuilder.RegisterType<RequestService>().As<IRequestService>();
            //_containerBuilder.RegisterType<DefaultBrowserCookiesService>().As<IBrowserCookiesService>();
            //_containerBuilder.RegisterType<GravatarUrlProvider>().As<IAvatarUrlProvider>();
            //_containerBuilder.RegisterType(typeof(SettingsService)).As(typeof(ISettingsService<RemoteSettings>));

            //if (AppSettings.UseFakes)
            //{
            //    _containerBuilder.RegisterType<FakeBookingService>().As<IBookingService>();
            //    _containerBuilder.RegisterType<FakeHotelService>().As<IHotelService>();
            //    _containerBuilder.RegisterType<FakeNotificationService>().As<INotificationService>();
            //    _containerBuilder.RegisterType<FakeSuggestionService>().As<ISuggestionService>();
            //}
            //else
            //{
            //    _containerBuilder.RegisterType<BookingService>().As<IBookingService>();
            //    _containerBuilder.RegisterType<HotelService>().As<IHotelService>();
            //    _containerBuilder.RegisterType<NotificationService>().As<INotificationService>();
            //    _containerBuilder.RegisterType<SuggestionService>().As<ISuggestionService>();
            //}

            //_containerBuilder.RegisterType<BookingCalendarViewModel>();
            //_containerBuilder.RegisterType<BookingHotelViewModel>();
            //_containerBuilder.RegisterType<BookingHotelsViewModel>();
            //_containerBuilder.RegisterType<BookingViewModel>();
            //_containerBuilder.RegisterType<CheckoutViewModel>();
            //_containerBuilder.RegisterType<HomeViewModel>();
            //_containerBuilder.RegisterType<LoginViewModel>();
            //_containerBuilder.RegisterType<MainViewModel>();
            //_containerBuilder.RegisterType<MenuViewModel>();
            //_containerBuilder.RegisterType<MyRoomViewModel>();
            //_containerBuilder.RegisterType<NotificationsViewModel>();
            //_containerBuilder.RegisterType<OpenDoorViewModel>();
            //_containerBuilder.RegisterType<SuggestionsViewModel>();

            //_containerBuilder.RegisterType(typeof(SettingsViewModel<RemoteSettings>));
            //_containerBuilder.RegisterType<ExtendedSplashViewModel>();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        //public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        //{
        //    _containerBuilder.RegisterType<TImplementation>().As<TInterface>();
        //}

        //public void Register<T>() where T : class
        //{
        //    _containerBuilder.RegisterType<T>();
        //}

        public void Build()
        {
            
        }
    }
}
