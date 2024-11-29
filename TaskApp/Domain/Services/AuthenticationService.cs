using AutoMapper;
using System.Security.Claims;
using TaskApp.Domain.Interfaces;
using TaskApp.Domain.Models;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;
using TaskApp.Infrastructure.Tools;
using TaskApp.Interfaces.Auth;
using TaskApp.Interfaces.Repositories;
using TaskApp.Models.DTOS.Request;
using TaskApp.Models.DTOS.Response;
using TaskApp.Models.Responses;

namespace TaskApp.Domain.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        public AuthenticationService(IUserRepository userRepository, IMapper mapper, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<OperationResult<string>> Login(UserLoginDTO loginUser)
        {
            var users = await _userRepository.FindUsers(user => user.Username == loginUser.Username);

            // Check if user exists
            var foundUser = users.FirstOrDefault();
            if (foundUser == null)
            {
                return OperationResult<string>.FailureResult(ResponseMessages.USER_NOT_FOUND);
            }

            // Compare foundUser password with received password:
            var validPassword = PasswordHash.VerifyPassword(loginUser.Password, foundUser.Password);
            if (validPassword == false)
            {
                // We use not found user message. We can't say to possible hackers the user is ok but the password is not
                return OperationResult<string>.FailureResult(ResponseMessages.USER_NOT_FOUND);
            }

            // Generate token
            var token = _jwtService.GenerateJwtToken(foundUser.Id.ToString());
            if (token == null)
            {
                return OperationResult<string>.FailureResult(ResponseMessages.SYSTEM_ERROR);
            }

            return OperationResult<string>.SuccessResult(token, ResponseMessages.AUTH_SUCCESS);
        }

        public Task<OperationResult<string>> Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<User?>> Me(string token)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(token);
            if (principal == null)
            {
                return OperationResult<User?>.FailureResult(ResponseMessages.NOT_VALID_TOKEN);
            }

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                var userId = long.Parse(userIdClaim.Value);
                var user = await _userRepository.GetUserById(userId);

                if (user == null)
                {
                    return OperationResult<User?>.FailureResult(ResponseMessages.USER_NOT_FOUND);
                }

                return OperationResult<User?>.SuccessResult(user, ResponseMessages.USER_FOUND);
            }
            else
            {
                return OperationResult<User?>.FailureResult(ResponseMessages.NOT_VALID_TOKEN);
            }
        }

        public async Task<OperationResult<string?>> Register(UserRegisterDTO newUser)
        {
            // Figure if the username already exists
            var foundUser = await _userRepository.FindUsers(user => user.Username == newUser.Username);

            // Check if user already exists by username
            if (foundUser.Any())
            {
                return OperationResult<string?>.FailureResult(ResponseMessages.USER_ALREADY_EXISTS);
            }

            // At this point the user does not exists, so we create it
            var UserMapping = _mapper.Map<User>(newUser);
            var result = await _userRepository.AddUser(UserMapping);

            if (!result.Success)
            {
                return OperationResult<string?>.FailureResult(result.Message);
            }

            return OperationResult<string?>.SuccessResult(null, result.Message);
        }
    }
}
