using System.Net;
using System.Security;
using System.Threading.Tasks;
using WPFPrism.Infrastructure.Models;

namespace WPFPrism.Infrastructure.Services.Interface
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(string username, string password);
        Task<bool> LoginAsync(string username, string password);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(string username, string newPassword);
        Task<bool> DeleteUserAsync(string username);
        Task<bool> IsUserAuthenticated();

    }
}
