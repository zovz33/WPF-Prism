using Microsoft.EntityFrameworkCore;
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

                // Извлекаем соль из базы данных для данного пользователя
                string salt = user.Salt;
                 
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

                // Сравниваем хешированный пароль с хешем из базы данных
                if (hashedPassword == user.Password)
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
                // Обработка ошибок
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

                // Генерируйте соль для пароля
                var salt = BCrypt.Net.BCrypt.GenerateSalt();

                // Сохраните соль в базе данных
                var user = new User { UserName = username, Salt = salt };
                await _dbContext.Users.AddAsync(user);

                // Создайте хеш пароля, включая соль
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

                // Сохраните хеш пароля в базе данных
                user.Password = hashedPassword;

                return await _dbContext.SaveChangesAsync() > 0 ? "Регистрация прошла успешно" : "Ошибка при регистрации";
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                return "Ошибка при регистрации";
            }
        }


        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
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
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
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
