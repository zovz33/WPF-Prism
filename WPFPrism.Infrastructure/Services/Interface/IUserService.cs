using System.Net;
using System.Security;
using System.Threading.Tasks;
using WPFPrism.Infrastructure.Models;

namespace WPFPrism.Infrastructure.Services.Interface
{
    public interface IUserService
    {
        event EventHandler AuthenticationStatusChanged;  
        User CurrentUser { get; }
        bool IsAuthenticated { get; }

        Task<string> LoginAsync(string username, string password);
        Task<string> RegisterAsync(string username, string password);
        void Logout();


        Task<List<User>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(string username, string newPassword);
        Task<bool> DeleteUserAsync(string username);
        Task<bool> CheckUserAuthentication();



    }
}
