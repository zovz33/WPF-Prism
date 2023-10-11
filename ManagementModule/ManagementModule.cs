using WPFPrism.ManagementModule.ViewModels;
using WPFPrism.ManagementModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace WPFPrism.ManagementModule
{
    public class ManagementModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ManageView, ManageViewModel>();
        }
    }
}