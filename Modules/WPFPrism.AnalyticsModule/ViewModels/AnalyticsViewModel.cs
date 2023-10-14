using Prism.Regions;
using System;
using WPFPrism.Infrastructure.Base;

namespace WPFPrism.AnalyticsModule.ViewModels
{
    public class AnalyticsViewModel : RegionViewModelBase
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

        public AnalyticsViewModel(IRegionManager regionManager) : base(regionManager)
        {
            _regionManager = regionManager;
        }
    }
}
