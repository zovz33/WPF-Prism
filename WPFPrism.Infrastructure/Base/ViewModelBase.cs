using Prism.Mvvm;
using Prism.Navigation;


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
