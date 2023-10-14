using WPFPrism.AuthModule.ViewModels;
using WPFPrism.AuthModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace WPFPrism.AuthModule
{
    public class AuthModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AuthView, AuthViewModel>();
            containerRegistry.RegisterForNavigation<RegistrView, RegistrViewModel>();
        }
    }
}