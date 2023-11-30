using Prism.Mvvm;

namespace WPFPrism.Infrastructure.Models
{
    public class UpdateDialogField : BindableBase
    {
        /*public string EditValueText { get; set; }
        public string EditNameHint { get; set; } 
        public string EditErrorMessage { get; set; }*/
        private string _editValueText;
        public string EditValueText
        {
            get { return _editValueText; }
            set
            {
                if (_editValueText != value)
                {
                    _editValueText = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _editNameHint;
        public string EditNameHint
        {
            get { return _editNameHint; }
            set
            {
                if (_editNameHint != value)
                {
                    _editNameHint = value;
                    RaisePropertyChanged();
                }
            }
        }
        
    }
}
