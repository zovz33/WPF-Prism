using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Windows;
using WPFPrism.Infrastructure.Base;
using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrism.AuthModule.ViewModels
{
    public class RegistrViewModel : RegionViewModelBase
    {

        #region Fields
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private IRegionNavigationJournal _journal;
        private readonly IUserService _userService;
        #endregion

        #region Properties

        private string _login;
        public string UserName
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
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

        public bool IsUseRequest { get; set; }

        #endregion

        #region Commands


        private DelegateCommand _navigateAuthCommand;
        public DelegateCommand NavigateAuthCommand =>
            _navigateAuthCommand ?? (_navigateAuthCommand = new DelegateCommand(ExecuteNavigateAuthCommand));

        private DelegateCommand _verityCommand;
        public DelegateCommand VerityCommand =>
            _verityCommand ?? (_verityCommand = new DelegateCommand(ExecuteVerityCommand));


        #endregion

        #region Excutes 

        void ExecuteNavigateAuthCommand()
        {
            Navigate("AuthView");
        }

        public async void ExecuteVerityCommand()
        {
            if (!RegisterValidation())
            {
                return;
            }
            this.IsUseRequest = true;
            try
            {
                string registrationResult = await _userService.RegisterAsync(this.UserName, this.Password);
                if (registrationResult == "Пользователь с таким именем уже существует")
                {
                    _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Пользователь с таким именем уже существует."}"), null);
                }
                else if (registrationResult == "Регистрация прошла успешно")
                {
                    _dialogService.ShowDialog("SuccessDialogView", new DialogParameters($"message={$"Поздравляем с регистрацией, {UserName}"}"), null);
                }
                else
                {
                    _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Ошибка при регистрации..."}"), null);
                }
            }
            catch (Exception ex)
            {
                _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Ошибка взаимодействия с базой данных..."}"), null);
                Console.WriteLine($"Ошибка при регистрации: {ex}");
                MessageBox.Show(ex.Message);
            }
        }



        #endregion

        public RegistrViewModel(IRegionManager regionManager, IUserService userService, IDialogService dialogService) : base(regionManager)
        {
            _regionManager = regionManager;
            _userService = userService;
            _dialogService = dialogService;
        }

        private bool RegisterValidation()
        {
            if (string.IsNullOrEmpty(this.UserName))
            {
                _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Поле логина пусто!"}"), null);
                return false;
            }
            //
            if (string.IsNullOrEmpty(this.Password))
            {
                _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Поле пароля пусто!"}"), null);
                return false;
            }
            if (string.IsNullOrEmpty(this.ConfirmPassword))
            {
                _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Вы не повторили пароль!"}"), null);
                return false;
            }
            if (Password.Trim() != ConfirmPassword.Trim())
            {
                _dialogService.Show("WarningDialogView", new DialogParameters($"message={"Пароли различны!"}"), null);
                return false;
            }

            return true;
        }


        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.Regions["ContentRegion"].RequestNavigate(navigatePath);
        }


        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback) // При перемещении
        {
            if (!string.IsNullOrEmpty(UserName) && this.IsUseRequest)
            {
                _dialogService.ShowDialog("AlertDialog", new DialogParameters($"message={"Войти ли под зарегистрированным пользователем"}"), r =>
                {
                    if (r.Result == ButtonResult.Yes)
                        navigationContext.Parameters.Add("Login", UserName);
                });
            }
            continuationCallback(true);
        }

    }
}
