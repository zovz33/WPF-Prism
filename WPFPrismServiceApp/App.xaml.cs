using WPFPrism.AuthModule;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System.Windows;
using WPFPrism.HomeModule;
using WPFPrism.ServiceModule;
using WPFPrismServiceApp.ViewModels;
using WPFPrismServiceApp.Views;
using WPFPrism.ManagementModule;
using WPFPrism.AnalyticsModule;

namespace WPFPrismServiceApp
{

    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<MainWindow, MainViewModel>();
            ViewModelLocationProvider.Register<NavigationControl, NavigationViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<HomeModule>();       // Home
            moduleCatalog.AddModule<ServiceModule>();    // View of Service
            moduleCatalog.AddModule<ManagementModule>(); // Data Management[For Admins]
            moduleCatalog.AddModule<AuthModule>();       // Authentification x Registration
            moduleCatalog.AddModule<AnalyticsModule>();  // Analytics/Statistics [For Admins]
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
