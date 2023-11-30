using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WPFPrism.Infrastructure.Models;
using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrism.ManagementModule.ViewModels
{
    public class UsersManageViewModel : BindableBase
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;
        #endregion

        #region Properties

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        #endregion

        #region Commands
        private DelegateCommand _addUserCommand;
        public DelegateCommand AddUserCommand =>
            _addUserCommand ?? (_addUserCommand = new DelegateCommand(ExecuteAddUserCommand));

        private DelegateCommand _editUserCommand;
        public DelegateCommand EditUserCommand =>
            _editUserCommand ?? (_editUserCommand = new DelegateCommand(ExecuteEditUserCommand));

        private DelegateCommand _deleteUserCommand;
        public DelegateCommand DeleteUserCommand =>
            _deleteUserCommand ?? (_deleteUserCommand = new DelegateCommand(ExecuteDeleteUserCommand));


        #endregion

        #region  Excutes

        private async void ExecuteAddUserCommand()
        {
            var userProperties = typeof(User).GetProperties()
                .Where(property => property.Name != "Salt" && property.Name != "Id")
                .ToDictionary(property => property.Name, property => "");

            // Создаем список полей для диалогового окна
            var fields = userProperties.Select(p => new UpdateDialogField
            {
                EditValueText = p.Value, // Пример: John
                EditNameHint = p.Key,    // Пример: Name 
            }).ToList();

            // Передаем в AddDialog
            var dialogParameters = new DialogParameters
            {
                { "Fields", fields },
                { "Message", "Добавление нового пользователя" }
            };

            // Открываем диалоговое окно
            _dialogService.ShowDialog("AddDialogView", dialogParameters, async r =>
            {
                if (r.Result == ButtonResult.Yes)
                {
                    // Получаем данные из диалога
                    var addedFields = r.Parameters.GetValue<List<UpdateDialogField>>("AddedUser");

                    var newUser = new User();

                    foreach (var field in addedFields)
                    {
                        var property = typeof(User).GetProperty(field.EditNameHint);
                        if (property != null)
                        {
                            // Присваиваем значение свойства объекту newUser
                            property.SetValue(newUser, Convert.ChangeType(field.EditValueText, property.PropertyType));
                        }
                    }

                    // Добавляем пользователя
                    string result = await _userService.AddUserAsync(newUser);

                    if (result == "Добавление пользователя прошло успешно")
                    {
                        _dialogService.Show("SuccessDialogView", new DialogParameters($"message=Успешное добавление пользователя!"), null);
                        await LoadUsersAsync();
                    }
                    else
                    {
                        _dialogService.Show("WarningDialogView", new DialogParameters($"message={result}"), null);
                    }
                }
                else if (r.Result == ButtonResult.No)
                {
                    _dialogService.Show("SuccessDialogView", new DialogParameters($"message=Отмена добавления пользователя."), null);
                }
            });
        }


        private async void ExecuteEditUserCommand()
        {
            if (SelectedUser != null)
            {
                var userProperties = SelectedUser.GetType().GetProperties()
                    .Where(property => property.Name != "Salt") // Не отображаем эти поля сущности
                    .ToDictionary(property => property.Name, property => property.GetValue(SelectedUser)?.ToString() ?? string.Empty);
                if (userProperties.ContainsKey("Id"))
                {
                    var idValue = userProperties["Id"];                                                      // Если ключ "Id" найден, сохраняем его значение в переменную idValue
                    userProperties.Remove("Id");                                                             // Удаляем ID из словаря 
                    userProperties = userProperties.Prepend(new KeyValuePair<string, string>("Id", idValue)) // Добавляем ключ "Id" и его значение в начало словаря, чтобы он был первым 
                        .ToDictionary(p => p.Key, p => p.Value);
                }

                var fields = userProperties.Select(p => new UpdateDialogField
                {
                    EditValueText = p.Value, // Пример: John
                    EditNameHint = p.Key,    // Пример: Name 
                }).ToList();

                // Передаем в UpdateDialog
                var dialogParameters = new DialogParameters
                {
                    { "SelectedUser", SelectedUser },
                    { "Fields", fields },
                    { "Message", "Изменение данных пользователя" }
                };

                _dialogService.ShowDialog("UpdateDialogView", dialogParameters, async r =>
                {
                    if (r.Result == ButtonResult.Yes)
                    {
                        // Получаем измененные данные из диалога
                        var updatedFields = r.Parameters.GetValue<List<UpdateDialogField>>("UpdatedUser");

                        var updatedUser = new User();

                        foreach (var field in updatedFields)
                        {
                            var property = typeof(User).GetProperty(field.EditNameHint);
                            if (property != null)
                            {
                                // Присваиваем значение свойства объекту updatedUser
                                property.SetValue(updatedUser, Convert.ChangeType(field.EditValueText, property.PropertyType));
                            }
                        }

                        bool success = await _userService.UpdateUserAsync(updatedUser);


                        if (success)
                        {
                            _dialogService.Show("SuccessDialogView", new DialogParameters($"message=Успешное обновление пользователя!"), null);
                            await LoadUsersAsync();
                        }
                        else
                        {
                            _dialogService.Show("WarningDialogView", new DialogParameters($"message=Ошибка при обновлении пользователя"), null);
                        }
                    }
                    else if (r.Result == ButtonResult.No)
                    {
                        _dialogService.Show("SuccessDialogView", new DialogParameters($"message=Отмена редактирования пользователя."), null);
                    }
                });
            }
        }



        private async void ExecuteDeleteUserCommand()
        {
            if (SelectedUser != null)
            {
                bool deleted = await _userService.DeleteUserAsync(SelectedUser.Id);
                if (deleted)
                {
                    SelectedUser = null;
                    await LoadUsersAsync();
                    _dialogService.Show("SuccessDialogView", new DialogParameters($"message=Успешное удаление пользователя!"), null);
                }
                else
                {
                    _dialogService.Show("SuccessDialogView", new DialogParameters($"message=Ошибка при удалении пользователя!"), null);
                }
            }
        }


        #endregion

        #region Constructor

        public UsersManageViewModel(IUserService userService, IDialogService dialogService)
        {
            _userService = userService;
            _dialogService = dialogService;
            InitializeAsync();
        }

        #endregion

        private async void InitializeAsync()
        {
            await LoadUsersAsync();
        }

        // Метод для обновления списка пользователей
        private async Task LoadUsersAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                Users = new ObservableCollection<User>(users);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}

