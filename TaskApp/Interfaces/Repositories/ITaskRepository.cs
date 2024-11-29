using TaskApp.Domain.Models;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;

namespace TaskApp.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        public Task<TaskM?> GetTaskById(long id);
        public Task<IEnumerable<TaskM>> GetAllByUserId(long userId);
        public Task<OperationResult<TaskM?>> Add(TaskM task);
        public Task<OperationResult<TaskM?>> Update(TaskM task);
        public Task<OperationResult<TaskM?>> Delete(int id);
    }
}
