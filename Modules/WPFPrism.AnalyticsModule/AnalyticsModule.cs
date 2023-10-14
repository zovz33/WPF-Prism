using Prism.Ioc;
using Prism.Modularity;
using WPFPrism.AnalyticsModule.ViewModels;
using WPFPrism.AnalyticsModule.Views;

namespace WPFPrism.AnalyticsModule
{
    public class AnalyticsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AnalyticsView, AnalyticsViewModel>();
        }
    }
}