using TaskApp.Domain.Models;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;

namespace TaskApp.Domain.Interfaces
{
    public interface ITaskService
    {
        public Task<OperationResult<TaskM?>> UpdateTask(User identifiedUser, TaskM task);
    }
}
