using Task3.Models;

namespace Task3.Services.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    Task<TaskItem?> GetTaskByIdAsync(int id);
    Task<string> AddTaskAsync(string title, string description);
    Task<string> UpdateTaskStatusAsync(int id, bool isCompleted);
    Task<string> DeleteTaskAsync(int id);
}