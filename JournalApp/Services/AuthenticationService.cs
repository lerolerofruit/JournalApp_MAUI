using JournalApp.Data;
using JournalApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JournalApp.Services
{
    public class AuthenticationService
    {
        private readonly JournalContext _context;
        private User _currentUser;

        public AuthenticationService(JournalContext context)
        {
            _context = context;
        }

        public User CurrentUser => _currentUser;

        public async Task<bool> RegisterAsync(string username, string password)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (existingUser != null)
            {
                return false;
            }

            var salt = GenerateSalt();
            var passwordHash = HashPassword(password, salt);

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Salt = salt,
                CreatedAt = DateTime.Now,
                Theme = "Light"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return false;
            }

            var passwordHash = HashPassword(password, user.Salt);
            if (passwordHash == user.PasswordHash)
            {
                _currentUser = user;
                return true;
            }

            return false;
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public async Task<bool> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            if (_currentUser == null)
            {
                return false;
            }

            var oldPasswordHash = HashPassword(oldPassword, _currentUser.Salt);
            if (oldPasswordHash != _currentUser.PasswordHash)
            {
                return false;
            }

            var newSalt = GenerateSalt();
            var newPasswordHash = HashPassword(newPassword, newSalt);

            _currentUser.PasswordHash = newPasswordHash;
            _currentUser.Salt = newSalt;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasAnyUsersAsync()
        {
            return await _context.Users.AnyAsync();
        }

        public async Task UpdateThemeAsync(string theme)
        {
            if (_currentUser != null)
            {
                _currentUser.Theme = theme;
                await _context.SaveChangesAsync();
            }
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var bytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
