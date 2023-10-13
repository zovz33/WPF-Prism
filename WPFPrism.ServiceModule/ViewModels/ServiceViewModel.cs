using Prism.Mvvm;
using Prism.Regions;
using System;
using WPFPrism.Infrastructure.Base;

namespace WPFPrism.ServiceModule.ViewModels
{
    public class ServiceViewModel : RegionViewModelBase
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

        public ServiceViewModel(IRegionManager regionManager) : base(regionManager)
        {
            _regionManager = regionManager;
        }


    }
}
