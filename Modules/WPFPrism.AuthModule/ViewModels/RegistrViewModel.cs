using Prism.Regions;
using System;
using WPFPrism.Infrastructure.Base;

namespace WPFPrism.AuthModule.ViewModels
{
    public class RegistrViewModel : RegionViewModelBase
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

        public RegistrViewModel(IRegionManager regionManager) : base(regionManager)
        {
            _regionManager = regionManager;
        }
    }
}
