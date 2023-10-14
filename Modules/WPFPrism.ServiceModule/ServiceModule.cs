using Prism.Ioc;
using Prism.Modularity;
using WPFPrism.ServiceModule.ViewModels;
using WPFPrism.ServiceModule.Views;

namespace WPFPrism.ServiceModule
{
    public class ServiceModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ServiceView, ServiceViewModel>();
        }
    }
}