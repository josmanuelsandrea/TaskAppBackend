using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskApp.Domain.Interfaces;
using TaskApp.Infrastructure.Tools;
using TaskApp.Interfaces.Controllers;
using TaskApp.Interfaces.Repositories;
using TaskApp.Models.DTOS.Request;
using TaskApp.Models.DTOS.Response;
using TaskApp.Models.Responses;

namespace TaskApp.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase, IAuthController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public AuthController(
            IAuthenticationService authenticationService,
            IMapper mapper,
            IUserRepository userRepository
        )
        {
            _authenticationService = authenticationService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string?>>> Login([FromBody] UserLoginDTO model)
        {
            var result = await _authenticationService.Login(model);
            if (!result.Success)
            {
                return BadRequest(new ApiResponse<string>(false, result.Message, null!));
            }

            return Ok(new ApiResponse<string>(true, result.Message, result.Data!));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<ApiResponse<UserDTO?>>> Me()
        {
            var token = TokenHelper.GetBearerToken(Request);

            if (token == null)
            {
                return Unauthorized();
            }

            var result = await _authenticationService.Me(token);
            if (!result.Success || result.Data == null)
            {
                return BadRequest(new ApiResponse<UserDTO>(false, result.Message, null!));
            }

            var userDTO = _mapper.Map<UserDTO>(result.Data);

            return Ok(new ApiResponse<UserDTO>(true, ResponseMessages.USER_FOUND, userDTO));
        }

        [HttpPost("refresh")]
        public ActionResult Refresh([FromBody] dynamic model)
        {
            throw new NotImplementedException();
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDTO?>>> Register([FromBody] UserRegisterDTO model)
        {
            var result = await _authenticationService.Register(model);
            if (result.Success)
            {
                return Ok(new ApiResponse<UserDTO?>(true, result.Message, null));
            } else
            {
                return BadRequest(new ApiResponse<UserDTO?>(false, result.Message, null));
            }
        }
    }
}
