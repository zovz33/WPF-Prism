using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Prism.Services.Dialogs;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Cryptography;
using System.Windows;
using WPFPrism.Infrastructure.Base;
using WPFPrism.Infrastructure.Database;
using WPFPrism.Infrastructure.Models;
using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrism.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext; 
        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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
                    return "Авторизация успешна";
                }
                else
                {
                    return "Неверный пароль";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex}");
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
                var user = new User { UserName = username, Password = HashPassword(password) };
                await _dbContext.Users.AddAsync(user);

                return await _dbContext.SaveChangesAsync() > 0 ? "Регистрация прошла успешно" : "Ошибка при регистрации";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка, регистрация не удалась: {ex}");
                MessageBox.Show($"Ошибка, регистрация не удалась: {ex.Message}");
                return "Ошибка при регистрации";
            }
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

        public async Task<bool> IsUserAuthenticated()
        {
            // Реализация этого метода зависит от того, как вы управляете сессиями/токенами аутентификации в вашем приложении.
            throw new NotImplementedException();
        }

        private string HashPassword(string password)
        { 
            // Пример хеширования пароля с использованием PBKDF2
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltSize: 16, iterations: 100000))
            {
                byte[] hash = rfc2898DeriveBytes.GetBytes(20);
                byte[] salt = rfc2898DeriveBytes.Salt;

                return Convert.ToBase64String(salt) + "|" + Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        { 
            // Пример проверки хеша пароля с использованием PBKDF2
            string[] parts = hashedPassword.Split('|');
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] hash = Convert.FromBase64String(parts[1]);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterations: 100000))
            {
                byte[] testHash = rfc2898DeriveBytes.GetBytes(20);

                for (int i = 0; i < 20; i++)
                {
                    if (testHash[i] != hash[i])
                        return false;
                }

                return true;
            }
        }
    }
}
