using WPFPrismServiceApp.ViewModels;
using WPFPrismServiceApp.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System.Windows;

namespace WPFPrismServiceApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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
            // moduleCatalog.AddModule<NavigationService>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
