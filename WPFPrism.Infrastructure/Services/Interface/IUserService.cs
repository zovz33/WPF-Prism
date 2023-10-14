using System.Net;
using System.Security;
using WPFPrism.Infrastructure.Models;

namespace WPFPrism.Infrastructure.Services.Interface
{
    public interface IUserService
    {
        Task<bool> IsUserAuthenticated();
        Task<bool> RegisterAsync(string username, string password);
        Task<bool> LoginAsync(string username, string password); 

    }
}
