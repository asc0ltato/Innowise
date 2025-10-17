using Task3.Database;
using Task3.Database.Interfaces;
using Task3.Repositories;
using Task3.Repositories.Interfaces;
using Task3.Services;
using Task3.Services.Interfaces;

const string connectionString = "Data Source=.;Initial Catalog=Task3;Integrated Security=True;TrustServerCertificate=True";

var databaseInitializer = new DatabaseInitializer(connectionString);

await databaseInitializer.InitializeAsync();
IDbConnectionFactory connectionFactory = new SqlConnectionFactory(connectionString);
ITaskRepository taskRepository = new TaskRepository(connectionFactory);
ITaskService taskService = new TaskService(taskRepository); 

await RunApplication(taskService);

static async Task RunApplication(ITaskService taskService)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("-------------Task management-------------");
        Console.WriteLine("1. Add a task");
        Console.WriteLine("2. View all tasks");
        Console.WriteLine("3. Update the status task");
        Console.WriteLine("4. Delete task");
        Console.WriteLine("5. Exit");
        Console.Write("Select an action: ");

        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("Error: Please enter a valid number!");
            WaitForContinue();
            continue;
        }

        switch ((MenuChoice)choice)
        {
            case MenuChoice.AddTask:
                await AddTask(taskService);
                break;
            case MenuChoice.ViewAllTasks:
                await ViewAllTasks(taskService);
                break;
            case MenuChoice.UpdateTaskStatus:
                await UpdateTaskStatus(taskService);
                break;
            case MenuChoice.DeleteTask:
                await DeleteTask(taskService);
                break;
            case MenuChoice.Exit:
                Console.WriteLine("Goodbye!");
                return;
            default:
                Console.WriteLine("Bad choice! Please select 1-5.");
                WaitForContinue();
                break;
        }
    }
}

static async Task AddTask(ITaskService taskService)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("-------------Add a new task-------------");
    
        Console.Write("Enter the title: ");
        var title = Console.ReadLine()?.Trim(); 
    
        Console.Write("Enter the description: ");
        var description = Console.ReadLine()?.Trim();
    
        var result = await taskService.AddTaskAsync(title, description ?? "");
        Console.WriteLine(result);

        if (HandleResult(result)) break;
    }
}

static async Task ViewAllTasks(ITaskService taskService)
{
    Console.Clear();
    Console.WriteLine("-------------All Tasks List-------------");
    
    var tasks = await taskService.GetAllTasksAsync();
    
    if (!tasks.Any())
    {
        Console.WriteLine("No tasks found!");
        WaitForContinue();
        return;
    }

    foreach (var task in tasks)
    {
        var status = task.IsCompleted ? "Completed" : "Not completed";
        var statusColor = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
        
        Console.Write($"{task.Id}. {task.Title} [");
        Console.ForegroundColor = statusColor;
        Console.Write(status);
        Console.ResetColor();
        Console.WriteLine("]");
        Console.WriteLine($"Description: {task.Description}");
        Console.WriteLine($"Created: {task.CreatedAt:dd.MM.yyyy HH:mm}");
        Console.WriteLine();
    }
    WaitForContinue();
}

static async Task UpdateTaskStatus(ITaskService taskService)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("-------------Update task status-------------");
    
        var id = ReadInteger("Enter task ID: ");
        if (id == null) continue;
    
        Console.Write("New status (1 - completed, 0 - not completed): ");
        var statusInput = Console.ReadLine()?.Trim();
    
        if (statusInput != "0" && statusInput != "1")
        {
            Console.WriteLine("Error: Please enter 0 or 1!");
            WaitForContinue();
            continue;
        }
        
        bool newStatus = statusInput == "1";
    
        var result = await taskService.UpdateTaskStatusAsync(id.Value, newStatus);
        Console.WriteLine(result);
        
        if (HandleResult(result)) break;
    }
}

static async Task DeleteTask(ITaskService taskService)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("-------------Delete task-------------");
    
        var id = ReadInteger("Enter the task ID to delete: ");
        if (id == null) continue;
    
        var result = await taskService.DeleteTaskAsync(id.Value);
        Console.WriteLine(result);
        
        if (HandleResult(result)) break;
    }
}

static int? ReadInteger(string prompt)
{
    Console.Write(prompt);
    var input = Console.ReadLine();
    
    if (int.TryParse(input, out var result) && result > 0)
        return result;
    
    Console.WriteLine("Error: Please enter a valid positive number!");
    return null;
}

static void WaitForContinue()
{
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

static bool HandleResult(string result)
{
    if (result.Contains("Error:"))
    {
        WaitForContinue();
        return false;
    }
    
    WaitForContinue();
    return true;
}

enum MenuChoice
{
    AddTask = 1,
    ViewAllTasks = 2,
    UpdateTaskStatus = 3,
    DeleteTask = 4,
    Exit = 5
}