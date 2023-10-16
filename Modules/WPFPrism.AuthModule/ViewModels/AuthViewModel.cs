using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Windows;
using WPFPrism.Infrastructure.Base;
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

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }


        public bool KeepAlive => true;

        #endregion

        #region Commands
        private DelegateCommand _navigateRegisterCommand;
        public DelegateCommand NavigateRegisterCommand =>
            _navigateRegisterCommand ?? (_navigateRegisterCommand = new DelegateCommand(ExecuteNavigateRegisterCommand));

        private DelegateCommand _loginCommand;
        public DelegateCommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand(ExecuteLoginCommandAsync));


        #endregion

        public AuthViewModel(IRegionManager regionManager, IUserService userService, IDialogService dialogService) : base(regionManager, userService)
        {
            _regionManager = regionManager;
            _userService = userService;
            _dialogService = dialogService;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("UserName"))
            {
                UserName = navigationContext.Parameters["UserName"] as string;
            }
        }

        private void ShowErrorMessage(string message)
        {
            _dialogService.Show("WarningDialogView", new DialogParameters($"message={message}"), null);
        }

        #region Excutes

        public async void ExecuteLoginCommandAsync()
        {
            if (string.IsNullOrEmpty(this.UserName)) // Валидация поле логина
            {
                ShowErrorMessage("Поле логина пусто.");
                return;
            }

            if (string.IsNullOrEmpty(this.Password)) // Валидация поле пароля
            {
                ShowErrorMessage("Поле пароля пусто.");
                return;
            }
            try
            {
                string loginResult = await _userService.LoginAsync(UserName, Password);
                if (loginResult == "Авторизация успешна")
                {
                    _dialogService.Show("SuccessDialogView", new DialogParameters($"message=Приветствуем, {UserName}, вы успешно авторизировались!"), null);
                    _regionManager.Regions["ContentRegion"].RequestNavigate("HomeView");
                }
                else
                {
                    ShowErrorMessage(loginResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в методе: {ex}");
                MessageBox.Show(ex.Message);
            }
        }

        private void ExecuteNavigateRegisterCommand() //Навигация на RegistrView "Нет аккаунта?Создать"
        {
            _regionManager.RequestNavigate("ContentRegion", "RegistrView");
        }

        #endregion
    }
}

