using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Windows;
using WPFPrismServiceApp.Views;

namespace WPFPrismServiceApp.ViewModels
{
    public class NavigationViewModel : BindableBase
    {

        #region Fields
        private IRegionNavigationJournal _journal;
        private readonly IRegionManager _regionManager;
        #endregion

        #region Properties

        private bool _isCanExcute;
        public bool IsCanExcute
        {
            get { return _isCanExcute; }
            set { SetProperty(ref _isCanExcute, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand<string> NavigationCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }

        private DelegateCommand _goForwardCommand;
        public DelegateCommand GoForwardCommand =>
            _goForwardCommand ?? (_goForwardCommand = new DelegateCommand(ExecuteGoForwardCommand));
        public DelegateCommand ClearJournal { get; private set; }
        #endregion

        #region  Excutes
        private void ExecuteGoForwardCommand()
        {
            _journal.GoForward();
        }
        #endregion



        public NavigationViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
 
            NavigationCommand = new DelegateCommand<string>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack, CanGoBack); 
            ClearJournal = new DelegateCommand(Clear);
        }

        private void Navigate(string View)
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

        private void Refresh()
        {
            GoBackCommand.RaiseCanExecuteChanged();
            GoForwardCommand.RaiseCanExecuteChanged();
        }

        private void GoBack()
        {
            _journal.GoBack();
            Refresh();
        }

        private bool CanGoBack()
        {
            return _journal != null && _journal.CanGoBack;
        }

        private void GoForward()
        {
            _journal.GoForward();
            Refresh();
        }


        private bool CanGoForward()
        {
            return _journal != null && _journal.CanGoForward;
        }

        private void Clear()
        {
            if (_journal != null)
            {
                _journal.Clear();
            } 
            Refresh();
        }

 

    }
}