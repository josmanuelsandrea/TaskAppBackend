using Microsoft.EntityFrameworkCore;
using TaskApp.Domain.Models;
using TaskApp.Infrastructure.Persistence.PostgreSQL;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;
using TaskApp.Infrastructure.Tools;
using TaskApp.Interfaces.Repositories;

namespace TaskApp.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskappContext _db;
        public TaskRepository(TaskappContext db)
        {
            _db = db;
        }
        public async Task<OperationResult<TaskM?>> Add(TaskM task)
        {
            try
            {
                await _db.AddAsync(task);
                await _db.SaveChangesAsync();
                return OperationResult<TaskM?>.SuccessResult(null, ResponseMessages.OPERATION_SUCCESS);
            } catch (Exception)
            {
                return OperationResult<TaskM?>.FailureResult(ResponseMessages.SYSTEM_ERROR);
            }
        }

        public async Task<OperationResult<TaskM?>> Delete(int id)
        {
            var task = await _db.TasksM.FindAsync(id);
            if (task != null)
            {
                try
                {
                    _db.TasksM.Remove(task);
                    await _db.SaveChangesAsync();
                    return OperationResult<TaskM?>.SuccessResult(null, ResponseMessages.OPERATION_SUCCESS);
                } catch (Exception)
                {
                    return OperationResult<TaskM?>.FailureResult(ResponseMessages.SYSTEM_ERROR);
                }
            }

            return OperationResult<TaskM?>.FailureResult(ResponseMessages.RESOURCE_NOT_FOUND);
        }

        public async Task<IEnumerable<TaskM>> GetAllByUserId(long userId)
        {
            var listOfTasks = await _db.TasksM.Where(task => task.UserId == userId).AsNoTracking().ToListAsync();
            return listOfTasks;
        }

        public async Task<TaskM?> GetTaskById(long id)
        {
            var task = await _db.TasksM.Where(task => task.Id == id).AsNoTracking().FirstOrDefaultAsync();
            if (task != null)
            {
                return task;
            }

            return null;
        }

        public async Task<OperationResult<TaskM?>> Update(TaskM task)
        {
            var foundTask = await _db.TasksM.FindAsync(task.Id);
            if (foundTask == null)
            {
                return OperationResult<TaskM?>.FailureResult(ResponseMessages.RESOURCE_NOT_FOUND);
            }
            try
            {
                foundTask.Status = task.Status;
                foundTask.Description = task.Description;
                foundTask.Title = task.Title;


                await _db.SaveChangesAsync();
                return OperationResult<TaskM?>.SuccessResult(null, ResponseMessages.OPERATION_SUCCESS);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return OperationResult<TaskM?>.FailureResult(ResponseMessages.SYSTEM_ERROR);
            }
        }
    }
}
