namespace Task1;

public static class DataProcessor
{
    public static string ProcessData(string dataName)
    {
        Console.WriteLine($"Synchronous process started: {dataName}");
        Thread.Sleep(3000);
        return $"Process {dataName} is completed in 3 seconds";
    }

    public static async Task<string> ProcessDataAsync(string dataName)
    {
        Console.WriteLine($"Asynchronous process started: {dataName}");
        await Task.Delay(3000);
        return $"Process {dataName} is completed in 3 seconds";
    }
}