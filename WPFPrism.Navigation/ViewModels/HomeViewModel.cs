using Prism.Mvvm;
using Prism.Regions;
using System;


namespace WPFPrism.HomeModule.ViewModels
{
    public class HomeViewModel : BindableBase, INavigationAware
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
