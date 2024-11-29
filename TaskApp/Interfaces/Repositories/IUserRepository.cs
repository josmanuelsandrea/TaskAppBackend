using TaskApp.Domain.Models;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;

namespace TaskApp.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetUserById(long id);
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<IEnumerable<User>> FindUsers(Func<User, bool> predicate);
        public Task<OperationResult<User?>> AddUser(User user);
        public Task<OperationResult<User?>> UpdateUser(User user);
        public Task<OperationResult<User?>> DeleteUserById(int id);
    }
}
