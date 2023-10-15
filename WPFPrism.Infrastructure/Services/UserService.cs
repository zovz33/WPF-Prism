using Microsoft.EntityFrameworkCore;
using Prism.Services.Dialogs;
using System.Security.Cryptography;
using WPFPrism.Infrastructure.Base;
using WPFPrism.Infrastructure.Database;
using WPFPrism.Infrastructure.Models;
using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrism.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context; 
        public UserService(ApplicationDbContext context)
        { 
            _context = context;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == username))
                throw new Exception("Пользователь уже существует");

            var user = new User { UserName = username, Password = HashPassword(password) };
            _context.Users.Add(user);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                User user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    throw new Exception("Пользователь не найден!");
                }
                if (!VerifyPassword(password, user.Password))
                {
                    throw new Exception("Неверный пароль!");
                }
                return true;
            }
            catch (Exception ex)
            {
                // Здесь можно обработать исключение или логировать его
                return false; // Возвращаем false в случае ошибки
            }
        }




        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(string username, string newPassword)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                throw new Exception("Пользователь не найден");

            user.Password = HashPassword(newPassword);
            _context.Users.Update(user);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                throw new Exception("Пользователь не найден");

            _context.Users.Remove(user);

            return await _context.SaveChangesAsync() > 0;
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
