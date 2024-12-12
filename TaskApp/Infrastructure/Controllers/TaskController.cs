using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskApp.Domain.Interfaces;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;
using TaskApp.Infrastructure.Tools;
using TaskApp.Interfaces.Controllers;
using TaskApp.Interfaces.Repositories;
using TaskApp.Models.DTOS.Request;
using TaskApp.Models.DTOS.Response;
using TaskApp.Models.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskApp.Infrastructure.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase, ITaskController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;

        public TaskController(IAuthenticationService authenticationService, IMapper mapper, ITaskService taskService)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
            _taskService = taskService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TaskDTO>>>> GetTasksByUser()
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null)
            {
                return Unauthorized(new ApiResponse<TaskDTO>(false, ResponseMessages.UNAUTHORIZED, null!));
            }

            var userTasks = await _taskService.GetTasksByUserID(user.Id);
            var mapping = _mapper.Map<IEnumerable<TaskDTO>>(userTasks);

            return Ok(new ApiResponse<IEnumerable<TaskDTO>>(true, ResponseMessages.OPERATION_SUCCESS, mapping));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<IEnumerable<TaskDTO>>>> AddTaskToUser([FromBody] TaskRequest task)
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null)
            {
                return Unauthorized(new ApiResponse<TaskDTO>(false, ResponseMessages.UNAUTHORIZED, null!));
            }

            var operation = await _taskService.AddNewTask(user.Id, task);
            if (!operation.Success)
            {
                return BadRequest(new ApiResponse<TaskDTO?>(false, operation.Message, null));
            }
            var dataMap = _mapper.Map<TaskDTO>(operation.Data);

            return Ok(new ApiResponse<TaskDTO?>(true, operation.Message, dataMap));
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<ApiResponse<TaskDTO>>> UpdateTask([FromBody] TaskUpdate task)
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null)
            {
                return Unauthorized(new ApiResponse<TaskDTO>(false, ResponseMessages.UNAUTHORIZED, null!));
            }

            var mappedTASK = _mapper.Map<TaskM>(task);
            var resultUpdate = await _taskService.UpdateTask(user, mappedTASK);

            if (!resultUpdate.Success)
            {
                return BadRequest(new ApiResponse<TaskDTO?>(false, resultUpdate.Message, null));
            }

            return Ok(new ApiResponse<TaskDTO?>(true, resultUpdate.Message, null));
        }
    }
}