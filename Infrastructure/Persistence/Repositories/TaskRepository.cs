using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskModel = Domain.Models.Task;

namespace Infrastructure.Persistence;

public class TaskRepository : ITaskRepository
{
    private readonly DatabaseContext _dbContext;

    public TaskRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TaskModel task)
    {
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TaskModel?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<TaskModel>> GetRangeAsync(int page, int pageSize)
    {
        return await _dbContext.Tasks
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var affected = await _dbContext
            .Tasks
            .Where(t => t.Id == id)
            .ExecuteDeleteAsync();
        if (affected == 0)
        {
            throw new KeyNotFoundException($"Task with ID {id} not found.");
        }
    }
}