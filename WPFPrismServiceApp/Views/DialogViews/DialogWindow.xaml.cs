using Prism.Services.Dialogs;
using System.Windows;

namespace WPFPrismServiceApp.Views.DialogViews
{
    /// <summary>
    /// Логика взаимодействия для DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window, IDialogWindow
    {
        public DialogWindow()
        {
            InitializeComponent();
        }
        public IDialogResult Result { get; set; }
    }
}
