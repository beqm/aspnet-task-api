using TaskModel = Domain.Models.Task;

namespace Domain.Interfaces;

public interface ITaskRepository
{
    Task AddAsync(TaskModel task);
    Task<TaskModel?> GetByIdAsync(Guid id);
    Task<IEnumerable<TaskModel>> GetRangeAsync(int page, int pageSize);
    Task SaveChangesAsync();
    Task DeleteAsync(Guid id);
}