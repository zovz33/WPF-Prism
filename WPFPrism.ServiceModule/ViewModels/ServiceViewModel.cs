using Prism.Mvvm;
using Prism.Regions;
using System;


namespace WPFPrism.ServiceModule.ViewModels
{
    public class ServiceViewModel : BindableBase, INavigationAware
    {



        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //    navigationContext.Parameters.Add("Person", Person);
        }
    }
}
