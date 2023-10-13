using Prism.Regions;
using System;
using WPFPrism.Infrastructure.Base;

namespace WPFPrism.AuthModule.ViewModels
{
    public class AuthViewModel : RegionViewModelBase
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

        public AuthViewModel(IRegionManager regionManager) : base(regionManager)
        {
            _regionManager = regionManager;
        }
    }
}
