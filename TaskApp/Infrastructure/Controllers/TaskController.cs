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
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;

        public TaskController(IAuthenticationService authenticationService, ITaskRepository taskRepository, IMapper mapper, ITaskService taskService)
        {
            _authenticationService = authenticationService;
            _taskRepository = taskRepository;
            _mapper = mapper;
            _taskService = taskService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TaskDTO>>>> GetTasksByUser()
        {
            var token = TokenHelper.GetBearerToken(Request);
            if (token == null)
            {
                return Unauthorized();
            }
            var result = await _authenticationService.Me(token);
            if (!result.Success || result.Data == null)
            {
                return BadRequest(new ApiResponse<TaskDTO>(false, result.Message, null!));
            }

            var userTasks = await _taskRepository.GetAllByUserId(result.Data.Id);
            var mapping = _mapper.Map<IEnumerable<TaskDTO>>(userTasks);

            return Ok(new ApiResponse<IEnumerable<TaskDTO>>(true, ResponseMessages.OPERATION_SUCCESS, mapping));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<IEnumerable<TaskDTO>>>> AddTaskToUser([FromBody] TaskRequest task)
        {
            var token = TokenHelper.GetBearerToken(Request);
            if (token == null)
            {
                return Unauthorized();
            }
            var result = await _authenticationService.Me(token);
            if (!result.Success || result.Data == null)
            {
                return BadRequest(new ApiResponse<TaskDTO>(false, result.Message, null!));
            }

            var TaskMapped = _mapper.Map<TaskM>(task);
            TaskMapped.UserId = result.Data.Id;

            var addTaskOperation = await _taskRepository.Add(TaskMapped);
            if (!addTaskOperation.Success)
            {
                return BadRequest(new ApiResponse<TaskDTO?>(false, addTaskOperation.Message, null));
            }
            var dataMap = _mapper.Map<TaskDTO>(addTaskOperation.Data);

            return Ok(new ApiResponse<TaskDTO?>(true, addTaskOperation.Message, dataMap));
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<ApiResponse<TaskDTO>>> UpdateTask([FromBody] TaskUpdate task)
        {
            var token = TokenHelper.GetBearerToken(Request);
            if (token == null)
            {
                return Unauthorized();
            }
            var result = await _authenticationService.Me(token);
            if (!result.Success || result.Data == null)
            {
                return BadRequest(new ApiResponse<TaskDTO>(false, result.Message, null!));
            }

            var mappedTASK = _mapper.Map<TaskM>(task);
            var resultUpdate = await _taskService.UpdateTask(result.Data, mappedTASK);

            if (!resultUpdate.Success)
            {
                return BadRequest(new ApiResponse<TaskDTO?>(false, resultUpdate.Message, null));
            }

            return Ok(new ApiResponse<TaskDTO?>(true, resultUpdate.Message, null));
        }
    }
}