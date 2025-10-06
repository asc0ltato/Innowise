namespace Task1;

public record CalculationResult(bool Success, double Result, string? ErrorMessage);

public static class Calculator
{
    public static CalculationResult Calculate(double x, double y, char operation)
    {
        try
        {
            double result = operation switch
            {
                '+' => x + y,
                '-' => x - y,
                '*' => x * y,
                '/' => y != 0 ? x / y : throw new DivideByZeroException("Division by zero!"),
                _ => throw new ArgumentException($"Unsupported operation: {operation}!")
            };

            return new CalculationResult(true, result, null);
        }
        catch (Exception ex)
        {
            return new CalculationResult(false, 0, ex.Message);
        }
    }
}
