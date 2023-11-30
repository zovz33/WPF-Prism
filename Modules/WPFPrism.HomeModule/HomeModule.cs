using Prism.Ioc;
using Prism.Modularity;
using WPFPrism.HomeModule.ViewModels;
using WPFPrism.HomeModule.Views;

namespace WPFPrism.HomeModule
{
    public class HomeModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>();
        }
    }
}