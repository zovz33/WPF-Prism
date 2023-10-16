using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Windows;
using WPFPrism.Infrastructure.Database;
using WPFPrism.Infrastructure.Models;
using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrism.Infrastructure.Services
{
    public class UserService : IUserService
    {
        public event EventHandler AuthenticationStatusChanged;  
        private readonly ApplicationDbContext _dbContext;
        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

            public User CurrentUser { get; private set; }
    public bool IsAuthenticated => CurrentUser != null;

    public async Task<string> LoginAsync(string username, string password)
    {
        try
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return "Пользователь не найден";
            }

            if (VerifyPassword(password, user.Password))
            {
                CurrentUser = user;
                AuthenticationStatusChanged?.Invoke(this, EventArgs.Empty);
                return "Авторизация успешна";
            }
            else
            {
                return "Неверный пароль";
            }
        }
        catch (Exception ex)
        {
        //    _logger.LogError(ex, "Ошибка при попытке авторизации");
            return "Ошибка при попытке авторизации";
        }
    }

    public async Task<string> RegisterAsync(string username, string password)
    {
        try
        {
            if (await _dbContext.Users.AnyAsync(u => u.UserName == username))
            {
                return "Пользователь с таким именем уже существует";
            }

            var hashedPassword = HashPassword(password);
            var user = new User { UserName = username, Password = hashedPassword };
            await _dbContext.Users.AddAsync(user);

            return await _dbContext.SaveChangesAsync() > 0 ? "Регистрация прошла успешно" : "Ошибка при регистрации";
        }
        catch (Exception ex)
        {
       //     _logger.LogError(ex, "Ошибка при регистрации");
            return "Ошибка при регистрации";
        }
    }

        public async Task<bool> CheckUserAuthentication()
        {
            return IsAuthenticated;
        }


        public void Logout()
        {
            CurrentUser = null;

            AuthenticationStatusChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(string username, string newPassword)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                throw new Exception("Пользователь не найден");

            user.Password = HashPassword(newPassword);
            _dbContext.Users.Update(user);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                throw new Exception("Пользователь не найден");

            _dbContext.Users.Remove(user);

            return await _dbContext.SaveChangesAsync() > 0;
        }

       
    }
}
