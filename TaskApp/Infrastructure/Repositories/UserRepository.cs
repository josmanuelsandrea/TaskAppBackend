using Microsoft.EntityFrameworkCore;
using TaskApp.Domain.Models;
using TaskApp.Infrastructure.Persistence.PostgreSQL;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;
using TaskApp.Infrastructure.Tools;
using TaskApp.Interfaces.Repositories;

namespace TaskApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskappContext _db;
        public UserRepository(TaskappContext db)
        {
            _db = db;
        }
        public async Task<OperationResult<User?>> AddUser(User user)
        {
            if (user == null)
            {
                return OperationResult<User?>.FailureResult("User cannot be null.");
            }

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                user.Password = PasswordHash.HashPassword(user.Password);
            }
            else
            {
                return OperationResult<User?>.FailureResult("Password cannot be null or empty");
            }

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return OperationResult<User?>.SuccessResult(null, "User added succesfully");
        }

        public async Task<OperationResult<User?>> DeleteUserById(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                return OperationResult<User?>.SuccessResult(null, "Deleted user correctly");
            }
            else
            {
                return OperationResult<User?>.FailureResult($"User with ID {id} not found.");
            }
        }

        public async Task<IEnumerable<User>> FindUsers(Func<User, bool> predicate)
        {
            return _db.Users.AsEnumerable().Where(predicate);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User> GetUserById(long id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            return user;
        }

        public async Task<OperationResult<User?>> UpdateUser(User user)
        {
            if (user == null)
            {
                return OperationResult<User?>.FailureResult("User cannot be null");
            }

            var existingUser = await _db.Users.FindAsync(user.Id);
            if (existingUser != null)
            {
                _db.Entry(existingUser).CurrentValues.SetValues(user);
                await _db.SaveChangesAsync();
                return OperationResult<User?>.SuccessResult(null, "User created succesfully");
            }
            else
            {
                return OperationResult<User?>.FailureResult($"User with ID {user.Id} not found.");
            }
        }
    }
}
