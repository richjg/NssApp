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

            /*
             Register up types here for DI
             In NavigationService.CreatePageViewModelMappings - Add Mapping For ViewModel To View
             */

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
