using Prism.Regions;

namespace WPFPrism.Infrastructure.Base
{
    public class RegionViewModelBase : ViewModelBase, INavigationAware, IConfirmNavigationRequest
    {
        private IRegionNavigationJournal _journal;
        protected IRegionManager RegionManager { get; private set; }

        public RegionViewModelBase(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }

        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
         
 


    }
}
