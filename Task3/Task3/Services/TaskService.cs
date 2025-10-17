using Task3.Models;
using Task3.Repositories.Interfaces;
using Task3.Services.Interfaces;

namespace Task3.Services;

public class TaskService : ITaskService
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
    
    public async Task<string> AddTaskAsync(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            return "Error: Title is required!";
        
        var task = new TaskItem()
        {
            Title = title,
            Description = description,
            IsCompleted = false,
            CreatedAt = DateTime.Now
        };

        var success = await _taskRepository.AddAsync(task);
        return success ? "The task was successfully added!" : "Error when add a task!";
    }

    public async Task<string> UpdateTaskStatusAsync(int id, bool isCompleted)
    {
        if (id <= 0)
            return "Error: Invalid ID!";
        
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            return "Error: Task not found!";

        task.IsCompleted = isCompleted;
        var success = await _taskRepository.UpdateAsync(task);
        
        return success ? "Task status updated successfully!" : "Error update a task status!";
    }

    public async Task<string> DeleteTaskAsync(int id)
    {
        if (id <= 0)
            return "Error: Invalid ID!";

        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            return "Error: Task not found!";

        var success = await _taskRepository.DeleteAsync(id);
        return success ? "The task was successfully deleted!" : "Error when delete a task!";
    }
}