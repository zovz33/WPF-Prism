using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
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

        private DelegateCommand _loginCommand;
        public DelegateCommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand(ExecuteLoginCommandAsync));


        #endregion

        #region Excutes

        public async void ExecuteLoginCommandAsync()
        {
            if (string.IsNullOrEmpty(this.CurrentUser.UserName)) // Валидация поле логина
            {
                _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Поле логина пусто!"}"), null);
                return;
            }

            if (string.IsNullOrEmpty(this.CurrentUser.Password)) // Валидация поле пароля
            {
                _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Поле пароля пусто!"}"), null);
                return;
            }

            try
            {
                string loginResult = await _userService.LoginAsync(this.CurrentUser.UserName, this.CurrentUser.Password);
                if (loginResult == "Авторизация успешна")
                {
                    _dialogService.Show("SuccessDialogView", new DialogParameters($"message={"Приветствуем, " + CurrentUser.UserName + " ,вы успешно авторизировались!"}"), null);
                    _regionManager.Regions["ContentRegion"].RequestNavigate($"HomeView");
                }
                else
                {
                    _dialogService.Show("WarningDialogView", new DialogParameters($"message={loginResult}"), null);
                }
            }
            catch (Exception ex)
            {
                _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Ошибка взаимодействия с базой данных..."}"), null);
                Console.WriteLine($"Ошибка: {ex}");
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

        #endregion


        public AuthViewModel(IRegionManager regionManager, IUserService userService, IDialogService dialogService) : base(regionManager)
        {
            _regionManager = regionManager;
            _userService = userService;
            _dialogService = dialogService;
        }

        public void OnNavigatedTo(NavigationContext navigationContext) // Действие при навигации с RegisterView, то есть после регистрации вызывается 
        {
            _journal = navigationContext.NavigationService.Journal;

            var Login = navigationContext.Parameters["Login"] as string;
            if (Login != null)
            {
                this.CurrentUser = new User() { UserName = Login };
            }
            LoginCommand.RaiseCanExecuteChanged();
        }
    }
}
