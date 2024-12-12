using TaskApp.Domain.Models;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;
using TaskApp.Models.DTOS.Request;

namespace TaskApp.Domain.Interfaces
{
    public interface ITaskService
    {
        public Task<IEnumerable<TaskM>> GetTasksByUserID(long userId);
        public Task<OperationResult<TaskM>> AddNewTask(long userId, TaskRequest newTask);
        public Task<OperationResult<TaskM?>> UpdateTask(User identifiedUser, TaskM task);
    }
}
