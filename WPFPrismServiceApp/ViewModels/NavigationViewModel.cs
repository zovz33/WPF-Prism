using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrismServiceApp.ViewModels
{
    public class NavigationViewModel : BindableBase, INavigationAware
    {

        #region Fields
        private IRegionNavigationJournal _journal;
        private readonly IRegionManager _regionManager;
        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;
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

        private bool _isAuthButtonsVisible;
        public bool IsAuthButtonsVisible
        {
            get { return _isAuthButtonsVisible; }
            set { SetProperty(ref _isAuthButtonsVisible, value); }
        }

        private bool _isNotAuthButtonsVisible;
        public bool IsNotAuthButtonsVisible
        {
            get { return _isNotAuthButtonsVisible; }
            set { SetProperty(ref _isNotAuthButtonsVisible, value); }
        }




        public bool KeepAlive => true;
        #endregion

        #region Commands
        private DelegateCommand<string> _navigationCommand;
        public DelegateCommand<string> NavigationCommand =>
            _navigationCommand ?? (_navigationCommand = new DelegateCommand<string>(ExecuteNavigateCommand));

        private DelegateCommand _logoutCommand;
        public DelegateCommand LogoutCommand =>
            _logoutCommand ?? (_logoutCommand = new DelegateCommand(ExecuteLogoutCommand));

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

        private void UpdateButtonVisibility() // Видимость кнопок в зависимости от состояния авторизации пользователя
        {
            if (_userService.IsAuthenticated)
            {
                IsAuthButtonsVisible = false;
                IsNotAuthButtonsVisible = true;
            }
            else
            {
                IsAuthButtonsVisible = true;
                IsNotAuthButtonsVisible = false;
            }
            if (_journal != null) 
            {
                _journal.Clear();
            }
        }

        private void ExecuteLogoutCommand()
        {
            string userName = _userService.CurrentUser?.UserName;
            if (_userService.IsAuthenticated)
            {
                _userService.Logout(); 
                _dialogService.Show("SuccessDialogView", new DialogParameters($"message={userName + ", вы успешно вышли!"}"), null);
                _regionManager.Regions["ContentRegion"].RequestNavigate($"AuthView");
                Refresh();

            }
            else
            {
                _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Вы не авторизованы!"}"), null);
            }
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

        public NavigationViewModel(IRegionManager regionManager, IUserService userService, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _userService = userService;
            _dialogService = dialogService;

            UpdateButtonVisibility();

            _userService.AuthenticationStatusChanged += (sender, args) => // Вызывается при регистрации/авторизации
            {
                UpdateButtonVisibility(); 
            };
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