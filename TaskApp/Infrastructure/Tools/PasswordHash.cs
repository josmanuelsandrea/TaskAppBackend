using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;

namespace TaskApp.Infrastructure.Tools
{
    public static class PasswordHash
    {
        public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);
        }

        public static string HashPassword(string enteredPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(enteredPassword);
        }
    }
}
