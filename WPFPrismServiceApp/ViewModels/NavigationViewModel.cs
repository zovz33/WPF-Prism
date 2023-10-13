using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Printing;
using System.Windows.Input;

namespace WPFPrismServiceApp.ViewModels
{
    public class NavigationViewModel : BindableBase, INavigationAware
    {

        #region Fields
        private IRegionNavigationJournal _journal;
        private readonly IRegionManager _regionManager;
        #endregion

        #region Properties

        private bool _isCanExcuteGoBack;
        public bool IsCanExcuteGoBack
        {
            get { return _isCanExcuteGoBack; }
            set { SetProperty(ref _isCanExcuteGoBack, value); }
        }

        private bool _isCanExcuteGoForward;
        public bool IsCanExcuteGoForward
        {
            get { return _isCanExcuteGoForward; }
            set { SetProperty(ref _isCanExcuteGoForward, value); }
        }
        public bool KeepAlive => true;
        #endregion

        #region Commands
        private DelegateCommand<string> _navigationCommand;
        public DelegateCommand<string> NavigationCommand =>
            _navigationCommand ?? (_navigationCommand = new DelegateCommand<string>(ExecuteNavigateCommand));

        private DelegateCommand _goBackCommand;
        public DelegateCommand GoBackCommand =>
            _goBackCommand ?? (_goBackCommand = new DelegateCommand(ExecuteGoBackCommand, CanExecuteGoBackCommand));

        private DelegateCommand _goForwardCommand;
        public DelegateCommand GoForwardCommand =>
            _goForwardCommand ?? (_goForwardCommand = new DelegateCommand(ExecuteGoForwardCommand, CanExecuteGoForwardCommand));

        #endregion

        #region  Excutes
        private void ExecuteNavigateCommand(string View)
        {
            View = $"{View}View";
            _regionManager.Regions["ContentRegion"].RequestNavigate(View, navigationCallback =>
            {
                if ((bool)navigationCallback.Result)
                {
                    _journal = navigationCallback.Context.NavigationService.Journal;
                    Refresh();
                }
                else
                {
                }
            });
        }

        void ExecuteGoBackCommand()
        {
            if (_journal != null) 
                _journal.GoBack();
            Refresh();
        }
        private void ExecuteGoForwardCommand()
        {
            if (_journal != null)
                _journal.GoForward();
            Refresh();
        }
        #endregion

        #region CanExecute

        private bool CanExecuteGoBackCommand()
        {
            return this.IsCanExcuteGoBack = _journal != null && _journal.CanGoBack;
        }

        private bool CanExecuteGoForwardCommand()
        {
            return this.IsCanExcuteGoForward = _journal != null && _journal.CanGoForward;
        }

        #endregion

        public NavigationViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private void Refresh()
        {
            GoBackCommand.RaiseCanExecuteChanged();
            GoForwardCommand.RaiseCanExecuteChanged();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { 
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        { 
            _journal = navigationContext.NavigationService.Journal;
        }



    }
}