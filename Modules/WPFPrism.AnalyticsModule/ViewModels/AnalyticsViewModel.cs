using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using WPFPrism.Infrastructure.Base;
using WPFPrism.Infrastructure.Services;
using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrism.AnalyticsModule.ViewModels
{
    public class AnalyticsViewModel : RegionViewModelBase
    {

        #region Fields
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private IRegionNavigationJournal _journal;
        private readonly IUserService _userService;
        #endregion

        #region Properties
        #endregion

        #region Commands
        #endregion

        #region Excutes
        #endregion

        public AnalyticsViewModel(IRegionManager regionManager, IUserService userService) : base(regionManager)
        {
            _regionManager = regionManager;

        }
    }
}
