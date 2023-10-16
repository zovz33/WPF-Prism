using Prism.Regions;
using System;
using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrism.Infrastructure.Base
{
    public class RegionViewModelBase : ViewModelBase, INavigationAware, IConfirmNavigationRequest
    {
        protected IRegionManager RegionManager { get; private set; }
        private readonly IUserService _userService;

        public RegionViewModelBase(IRegionManager regionManager, IUserService userService)
        {
            RegionManager = regionManager;
            _userService = userService;
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
 