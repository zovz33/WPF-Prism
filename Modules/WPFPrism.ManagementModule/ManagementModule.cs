using Prism.Ioc;
using Prism.Modularity;
using WPFPrism.ManagementModule.ViewModels;
using WPFPrism.ManagementModule.Views;

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
            containerRegistry.RegisterForNavigation<ServicesManageView, ServicesManageViewModel>();
            containerRegistry.RegisterForNavigation<UsersManageView, UsersManageViewModel>();
        }
    }
}