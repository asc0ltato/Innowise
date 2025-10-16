using Task3.Models;

namespace Task3.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(int id);
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<bool> AddAsync(TaskItem task);
    Task<bool> UpdateAsync(TaskItem task);
    Task<bool> DeleteAsync(int id);
}