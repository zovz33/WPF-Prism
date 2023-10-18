using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using WPFPrism.Infrastructure.Base;
using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrism.AuthModule.ViewModels
{
    public class RegistrViewModel : ViewModelBase 
    {

        #region Fields
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService; 
        private readonly IUserService _userService;
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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetProperty(ref _confirmPassword, value); }
        }

        #endregion

        #region Commands


        private DelegateCommand _navigateAuthCommand;
        public DelegateCommand NavigateAuthCommand =>
            _navigateAuthCommand ?? (_navigateAuthCommand = new DelegateCommand(ExecuteNavigateAuthCommand));

        private DelegateCommand _verifyCommand;
        public DelegateCommand RegistrCommand =>
            _verifyCommand ?? (_verifyCommand = new DelegateCommand(ExecuteRegistrationCommand));

        #endregion

        public RegistrViewModel(IRegionManager regionManager, IUserService userService, IDialogService dialogService)  
        {
            _regionManager = regionManager;
            _userService = userService;
            _dialogService = dialogService;
        }

        #region Excutes 

        private void ExecuteNavigateAuthCommand()
        {
            var parameters = new NavigationParameters();
            parameters.Add("UserName", UserName);
            _regionManager.RequestNavigate("ContentRegion", "AuthView", parameters);
        }

        private bool RegisterValidation()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                ShowErrorMessage("Поле логина пусто!");
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                ShowErrorMessage("Поле пароля пусто!");
                return false;
            }
            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                ShowErrorMessage("Вы не повторили пароль!");
                return false;
            }
            if (Password.Trim() != ConfirmPassword.Trim())
            {
                ShowErrorMessage("Пароли различны!");
                return false;
            }
            return true;
        }

        private void ShowErrorMessage(string message)
        {
            _dialogService.Show("WarningDialogView", new DialogParameters($"message={message}"), null);
        }

        private void ShowSuccessMessage(string message)
        {
            _dialogService.ShowDialog("SuccessDialogView", new DialogParameters($"message={message}"), null);
        }

        public async void ExecuteRegistrationCommand()
        {
            if (!RegisterValidation())
            {
                return;
            }

            try
            {
                await RegisterUser();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в ExecuteRegistrationCommand: {ex}");
                MessageBox.Show(ex.Message);
            }
        }

        private async Task AutoLogin()
        {
            string loginResult = await _userService.LoginAsync(UserName, Password);
            if (loginResult == "Авторизация успешна")
            {
                _regionManager.Regions["ContentRegion"].RequestNavigate("HomeView");
                ShowSuccessMessage($"Приветствуем, {UserName}, вы успешно авторизировались!");
            }
            else
            {
                ShowErrorMessage(loginResult);
            }
        }

        private async Task RegisterUser()
        {
            string registrationResult = await _userService.RegisterAsync(UserName, Password);
            if (registrationResult == "Пользователь с таким именем уже существует")
            {
                ShowErrorMessage(registrationResult);
            }
            else if (registrationResult == "Регистрация прошла успешно")
            {
                ShowSuccessMessage($"Пользователь {UserName} успешно зарегистрирован.");
                _dialogService.ShowDialog("AlertDialogView", new DialogParameters($"message=Хотите ли вы сразу авторизоваться?"), async r =>
                {
                    if (r.Result == ButtonResult.Yes)
                    {
                        await AutoLogin();
                    }
                    else if (r.Result == ButtonResult.No)
                    {
                        _regionManager.Regions["ContentRegion"].RequestNavigate("AuthView");
                    }
                });
            }
            else
            {
                ShowErrorMessage("Ошибка при регистрации...");
            }
        }

        #endregion
    }
}



