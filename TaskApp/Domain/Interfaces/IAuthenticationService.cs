using TaskApp.Domain.Models;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;
using TaskApp.Models.DTOS.Request;

namespace TaskApp.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<OperationResult<string?>> Register(UserRegisterDTO newUser);
        public Task<OperationResult<string>> Login(UserLoginDTO loginUser);
        public Task<OperationResult<User?>> Me(string token);
        public Task<OperationResult<string>> Logout();
    }
}
