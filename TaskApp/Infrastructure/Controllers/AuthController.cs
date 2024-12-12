using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskApp.Domain.Interfaces;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;
using TaskApp.Infrastructure.Tools;
using TaskApp.Interfaces.Controllers;
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

        public AuthController(
            IAuthenticationService authenticationService,
            IMapper mapper
        )
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        [AllowAnonymous]
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
            var user = HttpContext.Items["User"] as User;
            if (user == null)
            {
                return Unauthorized(new ApiResponse<TaskDTO>(false, ResponseMessages.UNAUTHORIZED, null!));
            }

            var userDTO = _mapper.Map<UserDTO>(user);

            return Ok(new ApiResponse<UserDTO>(true, ResponseMessages.USER_FOUND, userDTO));
        }

        [HttpPost("refresh")]
        public ActionResult Refresh([FromBody] dynamic model)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
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
