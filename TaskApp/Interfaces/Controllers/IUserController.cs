using Microsoft.AspNetCore.Mvc;
using TaskApp.Models.DTOS.Response;
using TaskApp.Models.Responses;

namespace TaskApp.Interfaces.Controllers
{
    public interface IUserController
    {
        Task<ActionResult<ApiResponse<UserDTO?>>> GetById(int userid);
    }
}
