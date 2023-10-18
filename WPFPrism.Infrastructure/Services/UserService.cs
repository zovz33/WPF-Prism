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
        private readonly ILoggerService _logger;

        public UserService(ApplicationDbContext dbContext, ILoggerService logger)
        {
            _dbContext = dbContext;
            _logger = logger;
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
                    _logger.Information("Попытка входа с несуществующим пользователем: {Username}", username);
                    return "Пользователь не найден";
                }

                string salt = user.Salt;
                string hashedPassword = HashPasswordWithSalt(password, salt);

                if (hashedPassword == user.Password)
                {
                    _logger.Information("Пользователь {Username} успешно вошел в систему", username);
                    CurrentUser = user;
                    AuthenticationStatusChanged?.Invoke(this, EventArgs.Empty);
                    return "Авторизация успешна";
                }
                else
                {
                    _logger.Information("Неверный ввод пароля у: {Username}", username);
                    return "Неверный пароль";
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при попытке авторизации для пользователя: {Username}", username);
                return "Ошибка при попытке авторизации";
            }
        }

        public async Task<string> RegisterAsync(string username, string password)
        {
            try
            {
                if (await _dbContext.Users.AnyAsync(u => u.UserName == username))
                {
                    _logger.Warning("Попытка регистрации с уже существующим именем пользователя: {Username}", username);
                    return "Пользователь с таким именем уже существует";
                }

                var salt = BCrypt.Net.BCrypt.GenerateSalt();
                var hashedPassword = HashPasswordWithSalt(password, salt);
                
                var user = new User { UserName = username, Password = hashedPassword, Salt = salt };
                await _dbContext.Users.AddAsync(user);
                int result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.Information("Пользователь {Username} успешно зарегистрирован", username);
                    return "Регистрация прошла успешно";
                }
                else
                {
                    _logger.Warning("Вот такая ошибка при регистрации пользователя: {Username}", username);
                    return "Ошибка при регистрации";
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при регистрации пользователя: {Username}", username);
                return "Ошибка при регистрации";
            }
        }

        private string HashPasswordWithSalt(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public async Task<bool> CheckUserAuthentication()
        {
            return IsAuthenticated;
        }

        public void Logout()
        {
            string username = CurrentUser?.UserName;
            CurrentUser = null;
            _logger.Information("Пользователь {Username} вышел из системы", username);
            AuthenticationStatusChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task<string> ChangePasswordAsync(string username, string currentPassword, string newPassword)
        {
            try
            {
                User user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);

                if (user == null)
                {
                    _logger.Warning("Попытка смены пароля для несуществующего пользователя: {Username}", username);
                    return "Пользователь не найден";
                }

                string salt = user.Salt;
                string currentHashedPassword = HashPasswordWithSalt(currentPassword, salt);

                if (currentHashedPassword == user.Password)
                {
                    // Текущий пароль прошел проверку
                    string newSalt = BCrypt.Net.BCrypt.GenerateSalt();
                    user.Salt = newSalt;
                    user.Password = HashPasswordWithSalt(newPassword, newSalt);
                    await _dbContext.SaveChangesAsync();

                    _logger.Information("Пользователь {Username} успешно сменил пароль", username);
                    return "Смена пароля прошла успешно";
                }
                else
                {
                    _logger.Warning("Неудачная попытка смены пароля для пользователя: {Username}", username);
                    return "Неверный текущий пароль";
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при смене пароля для пользователя: {Username}", username);
                return "Ошибка при смене пароля";
            }
        }

        public async Task<string> ChangeUsernameAsync(string currentUsername, string newUsername)
        {
            try
            {
                User user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == currentUsername);

                if (user == null)
                {
                    _logger.Warning("Попытка смены имени для несуществующего пользователя: {Username}", currentUsername);
                    return "Пользователь не найден";
                }

                if (await _dbContext.Users.AnyAsync(u => u.UserName == newUsername))
                {
                    _logger.Warning("Попытка смены имени на уже существующее имя: {NewUsername}", newUsername);
                    return "Пользователь с новым именем уже существует";
                }

                user.UserName = newUsername;
                await _dbContext.SaveChangesAsync();

                _logger.Information("Пользователь {Username} успешно сменил имя на {NewUsername}", currentUsername, newUsername);
                return "Смена имени прошла успешно";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при смене имени пользователя: {Username}", currentUsername);
                return "Ошибка при смене имени";
            }
        }

        public async Task<User> GetCurrentUserInformationAsync()
        {
            // Если пользователь аутентифицирован, вернем информацию о текущем пользователе
            if (IsAuthenticated)
            {
                return CurrentUser;
            }

            return null;
        }

        public async Task<bool> CheckUserPermissionsAsync(string username, List<string> permissions)
        {
            // Здесь вы можете реализовать проверку разрешений пользователя
            // на выполнение определенных действий в вашем приложении.
            // Этот метод будет зависеть от вашей системы разрешений и ролей.

            // Верните true, если у пользователя есть необходимые разрешения, и false в противном случае.
            return true;  
        }

        //

        public async Task<List<User>> GetAllUsersAsync()
        {
            _logger.Information("Запрос списка пользователей");
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(string username, string newPassword)
        {
            try
            {
                var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    _logger.Warning("Попытка обновления пароля для несуществующего пользователя: {Username}", username);
                    throw new Exception("Пользователь не найден");
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                int result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.Information("Пароль пользователя {Username} успешно обновлен", username);
                    return true;
                }
                else
                {
                    _logger.Warning("Ошибка при обновлении пароля для пользователя: {Username}", username);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при обновлении пароля для пользователя: {Username}", username);
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            try
            {
                var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);

                if (user == null)
                {
                    _logger.Warning("Попытка удаления несуществующего пользователя: {Username}", username);
                    throw new Exception("Пользователь не найден");
                }

                _dbContext.Users.Remove(user);
                int result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.Information("Пользователь {Username} успешно удален", username);
                    return true;
                }
                else
                {
                    _logger.Warning("Ошибка при удалении пользователя: {Username}", username);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при удалении пользователя: {Username}", username);
                return false;
            }
        }
    }
}