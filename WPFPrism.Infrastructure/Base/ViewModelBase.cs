using Prism.Mvvm;
using Prism.Navigation;
using System;


namespace WPFPrism.Infrastructure.Base
{
    public class ViewModelBase : BindableBase, IDestructible
    {
        protected ViewModelBase()
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
