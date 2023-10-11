using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using WPFPrism.HomeModule.Views;
using WPFPrismServiceApp.Views;

namespace WPFPrismServiceApp.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string _title = "МФЦ";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private readonly IRegionManager regionManager;

        public MainViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            regionManager.RegisterViewWithRegion("NavigationRegion", typeof(NavigationControl));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(HomeView));
        }



    }
}
