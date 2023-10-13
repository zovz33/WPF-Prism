using Prism.Regions;
using System;
using WPFPrism.Infrastructure.Base;

namespace WPFPrism.ManagementModule.ViewModels
{
    public class ManageViewModel : RegionViewModelBase
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

        public ManageViewModel(IRegionManager regionManager) : base(regionManager)
        {
            _regionManager = regionManager;
        }
    }
}
