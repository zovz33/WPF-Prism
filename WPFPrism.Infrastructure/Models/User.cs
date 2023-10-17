using System.Security;
using WPFPrism.Infrastructure.Base;

namespace WPFPrism.Infrastructure.Models
{
    public class User : ModelBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
