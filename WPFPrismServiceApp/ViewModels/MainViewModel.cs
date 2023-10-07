using WPFPrismServiceApp.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace WPFPrismServiceApp.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string _title = "МФЦ";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private readonly IRegionManager regionManager;
        //private readonly IDialogService _dialogService;

        public DelegateCommand<string> NavigationCommand { get; private set; }
        public DelegateCommand DialogCommand { get; private set; }

        public MainViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            // _dialogService = dialogService;
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(NavigationControl));
            NavigationCommand = new DelegateCommand<string>(Navigation);
            // DialogCommand = new DelegateCommand(ShowDialog);
        }

        private void Navigation(string content)
        {
            regionManager.Regions["ContentRegion"].RequestNavigate($"{content}View");
        }


    }
}
