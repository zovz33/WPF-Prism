using Prism.Regions;
using Prism.Services.Dialogs;
using WPFPrism.Infrastructure.Base;
using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrism.ServiceModule.ViewModels
{
    public class ServiceViewModel : ViewModelBase
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

        public ServiceViewModel(IRegionManager regionManager, IUserService userService)
        {
            _regionManager = regionManager;
            _userService = userService;
        }


    }
}
