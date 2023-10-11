using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows;

namespace WPFPrismServiceApp.ViewModels
{
    public class NavigationViewModel : BindableBase
    {
        private IRegionManager _regionManager;

        private IRegionNavigationService navigationService;
        private IRegionNavigationJournal _journal;

        public string NavigationParameters { get; set; }
        public string NavigationReceive { get; set; }
        public string NavigationRequest { get; set; }

        public DelegateCommand<string> NavigationCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
        public DelegateCommand ClearJournal { get; private set; }

        public NavigationViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigationParameters = nameof(NavigationParameters);
            NavigationReceive = nameof(NavigationReceive);
            NavigationRequest = nameof(NavigationRequest);
            NavigationCommand = new DelegateCommand<string>(Navigation);
            GoBackCommand = new DelegateCommand(GoBack, CanGoBack);
            GoForwardCommand = new DelegateCommand(GoForward, CanGoForward);
            ClearJournal = new DelegateCommand(Clear);
        }

        private void Navigation(string View)
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
        // ------------- Visibility

 

    }
}