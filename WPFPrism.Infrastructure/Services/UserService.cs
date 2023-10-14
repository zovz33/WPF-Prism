using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrism.Infrastructure.Services
{
    public class UserService : IUserService
    {
        public async Task<bool> RegisterAsync(string username, string password)
        {
            // Реализуйте логику регистрации здесь
            throw new NotImplementedException();
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            // Реализуйте логику авторизации здесь
            throw new NotImplementedException();
        }

        public async Task<bool> IsUserAuthenticated()
        {
            // Реализуйте проверку аутентификации пользователя здесь
            throw new NotImplementedException();
        }

    }
}
