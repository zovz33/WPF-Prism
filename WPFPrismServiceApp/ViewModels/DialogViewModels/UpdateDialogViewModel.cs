using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using WPFPrism.Infrastructure.Models;

namespace WPFPrismServiceApp.ViewModels.DialogViewModels
{
    public class UpdateDialogViewModel : BindableBase, IDialogAware
    {

        #region Fields 

        public event Action<IDialogResult> RequestClose;

        #endregion

        #region Properties 

        private string _title = "Уведомление";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private List<UpdateDialogField> _fields;
        public List<UpdateDialogField> Fields
        {
            get => _fields;
            set => SetProperty(ref _fields, value);
        }
        #endregion

        #region Commands

        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(ExecuteCloseDialogCommand));

        #endregion

        #region  Excutes

        void ExecuteCloseDialogCommand(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true") 
                result = ButtonResult.Yes; 
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.No;

            var dialogResult = new DialogResult(result, new DialogParameters { { "UpdatedUser", Fields } });
            RaiseRequestClose(dialogResult);
        }



        #endregion

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("Message");
            Fields = parameters.GetValue<List<UpdateDialogField>>("Fields");
        }
    }
}
