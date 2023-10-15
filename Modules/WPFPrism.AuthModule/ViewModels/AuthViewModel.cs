using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFPrism.Infrastructure.Base;
using WPFPrism.Infrastructure.Models;
using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrism.AuthModule.ViewModels
{
    public class AuthViewModel : RegionViewModelBase
    {

        #region Fields
        private IRegionNavigationJournal _journal;
        private readonly IRegionManager _regionManager;
        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;

        #endregion

        #region Properties

        private bool _isCanExcute;
        public bool IsCanExcute
        {
            get { return _isCanExcute; }
            set { SetProperty(ref _isCanExcute, value); }
        }

        private User _currentUser = new User();
        public User CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }
        }

        public bool KeepAlive => true;

        #endregion

        #region Commands
        private DelegateCommand<string> _createAccountCommand;
        public DelegateCommand<string> NavigationCommand =>
            _createAccountCommand ?? (_createAccountCommand = new DelegateCommand<string>(ExecuteNavigateCommand));

        private DelegateCommand<PasswordBox> _loginCommand;
        public DelegateCommand<PasswordBox> LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand<PasswordBox>(ExecuteLoginCommandAsync));


        #endregion

        #region Excutes

        public async void ExecuteLoginCommandAsync(PasswordBox passwordBox)
        {
            if (string.IsNullOrEmpty(this.CurrentUser.UserName))
            {
                _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Поле логина пустое!"}"), null);
                return;
            }

            this.CurrentUser.Password = passwordBox.Password;
            if (string.IsNullOrEmpty(this.CurrentUser.Password))
            {
                _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Поле пароля пустое!"}"), null);
                return;
            }

            try
            {
                bool isLoginSuccessful = await _userService.LoginAsync(this.CurrentUser.UserName, this.CurrentUser.Password);
                if (isLoginSuccessful == false)
                {
                    _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Логин и пароль неверны!"}"), null);
                    return;
                }
                else
                {
                    string View = $"HomeView";
                    _regionManager.Regions["ContentRegion"].RequestNavigate(View);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Ошибка: {ex.ToString()}";
                _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Ошибка взаимодействия с базой данных..."}"), null);
        
                Console.WriteLine(errorMessage);
            } 
        }




        // Навигация, вообщем то для кнопки "Нет аккаунта?Создать"
        private void ExecuteNavigateCommand(string View)
        {
            View = $"{View}View";
            _regionManager.Regions["ContentRegion"].RequestNavigate(View, navigationCallback =>
            {
                if ((bool)navigationCallback.Result)
                {
                }
                else
                {
                }
            });
        }

        private bool CanExecuteGoForwardCommand(PasswordBox passwordBox)
        {
            this.IsCanExcute = _journal != null && _journal.CanGoForward;
            return true;
        }

        #endregion


        public AuthViewModel(IRegionManager regionManager, IUserService userService, IDialogService dialogService) : base(regionManager)
        {
            _regionManager = regionManager;
            _userService = userService;
            _dialogService = dialogService; 
        }

        public void OnNavigatedTo(NavigationContext navigationContext) // Действие при навигации на другую страницу
        {
            _journal = navigationContext.NavigationService.Journal;

            var loginId = navigationContext.Parameters["loginId"] as string;
            if (loginId != null)
            {
                this.CurrentUser = new User() { UserName = loginId };
            }
            LoginCommand.RaiseCanExecuteChanged();
        }
    }
}
