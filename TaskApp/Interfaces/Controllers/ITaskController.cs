using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskApp.Models.DTOS.Request;
using TaskApp.Models.DTOS.Response;
using TaskApp.Models.Responses;

namespace TaskApp.Interfaces.Controllers
{
    public interface ITaskController
    {
        [Authorize]
        [HttpGet]
        Task<ActionResult<ApiResponse<IEnumerable<TaskDTO>>>> GetTasksByUser();

        [Authorize]
        [HttpPost]
        Task<ActionResult<ApiResponse<IEnumerable<TaskDTO>>>> AddTaskToUser([FromBody] TaskRequest task);

        [Authorize]
        [HttpPut]
        Task<ActionResult<ApiResponse<TaskDTO>>> UpdateTask([FromBody] TaskUpdate task);
    }
}
