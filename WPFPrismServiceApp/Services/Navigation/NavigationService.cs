using Prism.Commands;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using System;


namespace WPFPrismServiceApp.Services.Navigation
{
    public class NavigationService : BindableBase
    {
        private IRegionManager _regionManager;

        //private IRegionNavigationService navigationService;
        private IRegionNavigationJournal _journal;

        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public string NavigationParameters { get; set; }
        public string NavigationReceive { get; set; }
        public string NavigationRequest { get; set; }

        public DelegateCommand<string> NavigationCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
        public DelegateCommand ClearJournal { get; private set; }

        public NavigationService(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            Message = "Navigation Root from your Prism Module";
            NavigationParameters = nameof(NavigationParameters);
            NavigationReceive = nameof(NavigationReceive);
            NavigationRequest = nameof(NavigationRequest);
            NavigationCommand = new DelegateCommand<string>(Navigation); 
            GoBackCommand = new DelegateCommand(GoBack, CanGoBack);
            GoForwardCommand = new DelegateCommand(GoForward, CanGoForward); 
            ClearJournal = new DelegateCommand(Clear);
        }

        private void Navigation(string content)
        {
            content = $"{content}View";
            _regionManager.Regions["NavigationContent"].RequestNavigate(content, navigationCallback =>
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
            _journal.Clear();
            //Refresh();
        }
    }
}
