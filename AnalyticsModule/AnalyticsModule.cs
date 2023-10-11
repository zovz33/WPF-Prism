using WPFPrism.AnalyticsModule.ViewModels;
using WPFPrism.AnalyticsModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

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