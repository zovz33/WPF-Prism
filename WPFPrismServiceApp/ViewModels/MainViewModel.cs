using Prism.Mvvm;
using Prism.Regions;
using WPFPrism.AuthModule.Views;
using WPFPrismServiceApp.Views;

namespace WPFPrismServiceApp.ViewModels
{
    public class MainViewModel : BindableBase
    {

        #region Fields
        private readonly IRegionManager _regionManager;
        #endregion

        #region Properties
        private string _title = "МФЦ";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        #endregion

        #region Commands
        #endregion

        #region Excutes
        #endregion

        public MainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            regionManager.RegisterViewWithRegion("NavigationRegion", typeof(NavigationControl));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(AuthView));
        }



    }
}
