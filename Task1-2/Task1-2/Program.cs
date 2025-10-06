namespace Task1;

static class Program
{
    static async Task Main()
    {
        Console.WriteLine("Welcome! Enter the number:");
        Console.WriteLine("1 – Calculator");
        Console.WriteLine("2 – Parallel");

        var input = Console.ReadLine();
        
        switch (input)
        {
            case "1":
                Calculate();
                break;
            case "2":
                await Parallel();
                break;
            default:
                Console.WriteLine("Invalid choice! Please enter 1 or 2!");
                break;
        }
    }

    private static void Calculate()
    {
        Console.WriteLine("-----------Calculator-----------");

        bool continueCalculation = true;

        while (continueCalculation)
        {
            try
            {
                double firstNumber = GetNumber("\nEnter the first number:");
                char operation = GetOperation();
                double secondNumber = GetNumber("\nEnter the second number:");

                if (operation == '/' && !InputValidator.ValidateDivision(secondNumber))
                {
                    continue;
                }

                var result = Calculator.Calculate(firstNumber, secondNumber, operation);

                Console.WriteLine(result.Success
                    ? $"\nResult: {firstNumber} {operation} {secondNumber} = {result.Result}"
                    : $"Error: {result.ErrorMessage}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}\n");
            }

            continueCalculation = AskContinue();
        }
    }
    
    private static double GetNumber(string message)
    {
        while (true)
        {
            Console.WriteLine(message);
            string? input = Console.ReadLine();

            if (InputValidator.ValidateNumber(input ?? string.Empty, out double number))
            {
                return number;
            }
        }
    }

    private static char GetOperation()
    {
        while (true)
        {
            Console.WriteLine("\nChoose an arithmetic operation: '+', '-', '*', '/'");
            string? input = Console.ReadLine();

            if (InputValidator.ValidateOperation(input ?? string.Empty, out char operation))
            {
                return operation;
            }
        }
    }

    private static bool AskContinue()
    {
        while (true)
        {
            Console.WriteLine("\nDo you want to perform another calculation? (yes/no)");
            string? input = Console.ReadLine()?.ToLower();

            return input switch
            {
                "yes" => true,
                "no" => false,
                _ => ShowInvalidInputAndContinue()
            };
        }
    }

    private static bool ShowInvalidInputAndContinue()
    {
        Console.WriteLine("Please enter 'yes' or 'no'!");
        return true;
    }

    private static async Task Parallel()
    {
        Console.WriteLine("-----------Parallel-----------");
        
        Console.WriteLine("\nSYNCHRONOUS PROCESS");
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        var result1 = DataProcessor.ProcessData("File 1");
        Console.WriteLine(result1);
        
        var result2 = DataProcessor.ProcessData("File 2");
        Console.WriteLine(result2);
        
        var result3 = DataProcessor.ProcessData("File 3");
        Console.WriteLine(result3);
        
        stopwatch.Stop();
        Console.WriteLine($"\nTotal time synchronous process: {stopwatch.Elapsed.TotalSeconds:F2} sec");
        
        Console.WriteLine("\nASYNCHRONOUS PROCESS");
        
        stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        var task1 = DataProcessor.ProcessDataAsync("File 1");
        var task2 = DataProcessor.ProcessDataAsync("File 2");
        var task3 = DataProcessor.ProcessDataAsync("File 3");

        var tasks = new List<Task<string>> { task1, task2, task3 };

        while (tasks.Count > 0)
        {
            var completedTask = await Task.WhenAny(tasks);
            Console.WriteLine(completedTask.Result);
            tasks.Remove(completedTask);
        }
        
        stopwatch.Stop();
        Console.WriteLine($"\nTotal time asynchronous process: {stopwatch.Elapsed.TotalSeconds:F2} sec");
    }
}