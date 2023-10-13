using Prism.Regions;
using WPFPrism.Infrastructure.Base;

namespace WPFPrism.HomeModule.ViewModels
{
    public class HomeViewModel : RegionViewModelBase
    {

        #region Fields
        private readonly IRegionManager _regionManager;
        #endregion

        #region Properties
        #endregion

        #region Commands
        #endregion

        #region Excutes
        #endregion

        public HomeViewModel(IRegionManager regionManager) : base(regionManager)
        {
            _regionManager = regionManager;
        }
    }
}
