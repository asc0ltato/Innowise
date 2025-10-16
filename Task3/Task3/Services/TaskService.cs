using Task3.Models;
using Task3.Repositories.Interfaces;

namespace Task3.Services;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    
    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
    {
        return await _taskRepository.GetAllAsync();
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        return await _taskRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> AddTaskAsync(string title, string description)
    {
        var task = new TaskItem()
        {
            Title = title,
            Description = description,
            IsCompleted = false,
            CreatedAt = DateTime.Now
        };

        return await _taskRepository.AddAsync(task);
    }

    public async Task<bool> UpdateTaskCompletedAsync(int id, bool isCompleted)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) return false;

        task.IsCompleted = isCompleted;
        return await _taskRepository.UpdateAsync(task);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        return await _taskRepository.DeleteAsync(id);
    }
}