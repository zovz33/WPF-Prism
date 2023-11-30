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
        Task<string> AddUserAsync(User request);
        Task<bool> UpdateUserAsync(User request);
        Task<bool> DeleteUserAsync(int UserId);
        Task<bool> CheckUserAuthentication();

        Task<string> ChangePasswordAsync(string username, string currentPassword, string newPassword);
        Task<string> ChangeUsernameAsync(string currentUsername, string newUsername);
        Task<User> GetCurrentUserInformationAsync();
        Task<bool> CheckUserPermissionsAsync(string username, List<string> permissions);

    }
}
