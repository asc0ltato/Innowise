using Task3.Database;
using Task3.Database.Interfaces;
using Task3.Repositories;
using Task3.Repositories.Interfaces;
using Task3.Services;

const string connectionString = "Data Source=.;Initial Catalog=Task3;Integrated Security=True;TrustServerCertificate=True";

var databaseInitializer = new DatabaseInitializer(connectionString);

await databaseInitializer.InitializeAsync();
IDbConnectionFactory connectionFactory = new SqlConnectionFactory(connectionString);
ITaskRepository taskRepository = new TaskRepository(connectionFactory);
var taskService = new TaskService(taskRepository);

await RunApplication(taskService);
static async Task RunApplication(TaskService taskService)
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

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                await AddTask(taskService);
                break;
            case "2":
                await ViewAllTasks(taskService);
                break;
            case "3":
                await UpdateTaskStatus(taskService);
                break;
            case "4":
                await DeleteTask(taskService);
                break;
            case "5":
                Console.WriteLine("Goodbye!");
                return;
            default:
                Console.WriteLine("Bad choice!");
                break;
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}

static async Task AddTask(TaskService taskService)
{
    Console.Clear();
    Console.WriteLine("-------------Add a new task-------------");
    
    Console.Write("Enter the title: ");
    var title = Console.ReadLine();
    
    Console.Write("Enter the description: ");
    var description = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Error: The header is empty!");
        return;
    }

    var success = await taskService.AddTaskAsync(title, description ?? "");
    if (success)
        Console.WriteLine("The task was successfully added!!");
    else
        Console.WriteLine("Error when add a task!");
}

static async Task ViewAllTasks(TaskService taskService)
{
    Console.Clear();
    Console.WriteLine("-------------All Tasks List-------------");
    
    var tasks = await taskService.GetAllTasksAsync();
    
    if (!tasks.Any())
    {
        Console.WriteLine("No tasks found");
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
}

static async Task UpdateTaskStatus(TaskService taskService)
{
    Console.Clear();
    Console.WriteLine("-------------Update task status-------------");
    
    Console.Write("Enter task ID: ");
    if (!int.TryParse(Console.ReadLine(), out var id) || id <= 0)
    {
        Console.WriteLine("Error: Invalid ID!");
        return;
    }

    var task = await taskService.GetTaskByIdAsync(id);
    if (task == null)
    {
        Console.WriteLine("Error: Task not found!");
        return;
    }

    Console.WriteLine($"Current task: {task.Title}");
    Console.WriteLine($"Current status: {(task.IsCompleted ? "Completed" : "Not completed")}");
    
    Console.Write("New status (1 - completed, 0 - not completed): ");
    var statusInput = Console.ReadLine();
    
    bool newStatus = statusInput == "1";
    
    var success = await taskService.UpdateTaskCompletedAsync(id, newStatus);
    if (success)
        Console.WriteLine("Task status updated successfully!");
    else
        Console.WriteLine("Error update a task status!");
}

static async Task DeleteTask(TaskService taskService)
{
    Console.Clear();
    Console.WriteLine("-------------Delete task-------------");
    
    Console.Write("Enter the task ID to delete: ");
    if (!int.TryParse(Console.ReadLine(), out var id) || id <= 0)
    {
        Console.WriteLine("Error: Invalid ID!");
        return;
    }

    var task = await taskService.GetTaskByIdAsync(id);
    if (task == null)
    {
        Console.WriteLine("Error: The task wasn't found!");
        return;
    }

    Console.WriteLine($"Are you sure you want to delete the task: {task.Title}?");
    Console.Write("Confirm the deletion (y/n): ");
    var confirmation = Console.ReadLine()?.ToLower();

    if (confirmation != "y" && confirmation != "yes")
    {
        Console.WriteLine("Deletion cancelled");
        return;
    }

    var success = await taskService.DeleteTaskAsync(id);
    if (success)
        Console.WriteLine("The task was successfully deleted!");
    else
        Console.WriteLine("Error when delete a task!");
}