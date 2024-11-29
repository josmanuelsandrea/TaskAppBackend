using TaskApp.Domain.Interfaces;
using TaskApp.Domain.Models;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;
using TaskApp.Infrastructure.Tools;
using TaskApp.Interfaces.Repositories;

namespace TaskApp.Domain.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
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
