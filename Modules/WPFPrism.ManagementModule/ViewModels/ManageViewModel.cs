using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using WPFPrism.Infrastructure.Base;

namespace WPFPrism.ManagementModule.ViewModels
{
    public class ManageViewModel : ViewModelBase
    {

        #region Fields
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private IRegionNavigationJournal _journal;
        #endregion

        #region Properties

        #endregion

        #region Commands

        private DelegateCommand<string> _navigationCommand;
        public DelegateCommand<string> NavigationCommand =>
            _navigationCommand ?? (_navigationCommand = new DelegateCommand<string>(ExecuteNavigateCommand));

        #endregion

        #region Excutes

        private void ExecuteNavigateCommand(string view)
        {
            string targetView = $"{view}View";
            if (_journal == null || _journal.CurrentEntry == null || _journal.CurrentEntry.Uri.ToString() != targetView)
            {
                _regionManager.Regions["ManagementRegion"].RequestNavigate(targetView);
            }
        }

        #endregion

        public ManageViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
    }
}
