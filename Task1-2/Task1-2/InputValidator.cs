namespace Task1;

public static class InputValidator
{
    public static bool ValidateNumber(string input, out double number)
    {
        if (double.TryParse(input, out number))
        {
            return true;
        }
        
        Console.WriteLine("Error: enter a valid number, please!");
        return false;
    }

    public static bool ValidateOperation(string input, out char operation)
    {
        if (string.IsNullOrEmpty(input) || input.Length != 1)
        {
            operation = '\0';
            Console.WriteLine("Error enter a valid operation, please!");
            return false;
        }

        operation = input[0];
        char[] validOperations = ['+', '-', '*', '/'];

        if (validOperations.Contains(operation))
        {
            return true;
        }
        
        Console.WriteLine($"Error: unsupported operation '{operation}'!");
        return false;
    }

    public static bool ValidateDivision(double division)
    {
        if (division != 0) return true;
        Console.WriteLine("Error: division by zero isn't allowed!");
        return false;
    }
}