using Microsoft.AspNetCore.Mvc;
using TaskApp.Models.DTOS.Request;
using TaskApp.Models.DTOS.Response;
using TaskApp.Models.Responses;

namespace TaskApp.Interfaces.Controllers
{
    public interface IAuthController
    {
        [HttpPost]
        public Task<ActionResult<ApiResponse<string?>>> Login([FromBody] UserLoginDTO model);

        [HttpPost]
        public Task<ActionResult<ApiResponse<UserDTO?>>> Register([FromBody] UserRegisterDTO model);

        [HttpGet]
        public Task<ActionResult<ApiResponse<UserDTO?>>> Me();

        [HttpPost]
        public ActionResult Refresh([FromBody] dynamic model);
    }
}