using AutoMapper;
using System.Threading.Tasks;
using TaskApp.Domain.Interfaces;
using TaskApp.Domain.Models;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;
using TaskApp.Infrastructure.Tools;
using TaskApp.Interfaces.Repositories;
using TaskApp.Models.DTOS.Request;

namespace TaskApp.Domain.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        public TaskService(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskM>> GetTasksByUserID(long userId)
        {
            var userTasks = await _taskRepository.GetAllByUserId(userId);
            return userTasks;
        }

        public async Task<OperationResult<TaskM?>> AddNewTask(long userId, TaskRequest newTask)
        {
            var TaskMapped = _mapper.Map<TaskM>(newTask);
            TaskMapped.UserId = userId;

            var operation = await _taskRepository.Add(TaskMapped);
            return operation;
        }

        public async Task<OperationResult<TaskM?>> UpdateTask(User identifiedUser, TaskM task)
        {
            var foundTask = await _taskRepository.GetTaskById(task.Id);

            if (foundTask == null)
            {
                return OperationResult<TaskM?>.FailureResult(ResponseMessages.RESOURCE_NOT_FOUND);
            }

            if (foundTask.UserId != identifiedUser.Id)
            {
                return OperationResult<TaskM?>.FailureResult(ResponseMessages.UNAUTHORIZED);
            };

            var result = await _taskRepository.Update(task);
            if (!result.Success)
            {
                return OperationResult<TaskM?>.FailureResult(result.Message);
            }

            return OperationResult<TaskM?>.SuccessResult(null, ResponseMessages.OPERATION_SUCCESS);
        }
    }
}
