using System;
using System.Globalization;

class Calculator
{
    static void Main()
    {
        bool keepRunning = true;

        while (keepRunning)
        {
            ShowMenu();

            // Read choice
            Console.Write("> ");
            string choiceInput = Console.ReadLine()?.Trim() ?? "";
            if (!int.TryParse(choiceInput, out int choice) || choice < 1 || choice > 4)
            {
                Console.WriteLine("Invalid choice. Please enter 1, 2, 3 or 4.");
                continue;
            }

            // Read two numeric values (Value 1 and Value 2)
            double a = ReadDouble("Enter Value 1: ");
            double b = ReadDouble("Enter Value 2: ");

            double result;
            string opSymbol;

            switch (choice)
            {
                case 1:
                    result = Add(a, b);
                    opSymbol = "+";
                    break;
                case 2:
                    result = Subtract(a, b);
                    opSymbol = "-";
                    break;
                case 3:
                    result = Multiply(a, b);
                    opSymbol = "×";
                    break;
                case 4:
                    // Handle division by zero
                    if (b == 0)
                    {
                        Console.WriteLine("Division by zero is not allowed.");
                        goto AskContinue;
                    }
                    result = Divide(a, b);
                    opSymbol = "÷";
                    break;
                default:
                    // Shouldn't happen because of earlier validation
                    Console.WriteLine("Unknown operation.");
                    goto AskContinue;
            }

            // Print result in the format "A op B = C"
            // Example: 10 + 20 = 30
            Console.WriteLine($"{a} {opSymbol} {b} = {result}");

        AskContinue:
            // Prompt to continue or exit
            Console.Write("Do you want to continue again (Y/N)? ");
            string cont = Console.ReadLine()?.Trim() ?? "";
            if (cont.Equals("N", StringComparison.OrdinalIgnoreCase))
            {
                keepRunning = false;
            }
            // any other input (including 'Y' or empty) will continue
            Console.WriteLine();
        }

        Console.WriteLine("Calculator exited. Goodbye!");
    }

    static void ShowMenu()
    {
        Console.WriteLine("Press any following key to perform an arithmetic");
        Console.WriteLine("operation:");
        Console.WriteLine("1 - Addition");
        Console.WriteLine("2 - Subtraction");
        Console.WriteLine("3 - Multipliation");
        Console.WriteLine("4 - Division");
    }

    static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (double.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out double val))
            {
                return val;
            }
            Console.WriteLine("Invalid number. Please enter a valid numeric value (e.g., 12.34).");
        }
    }

    // Separate methods for each operation
    static double Add(double x, double y) => x + y;
    static double Subtract(double x, double y) => x - y;
    static double Multiply(double x, double y) => x * y;
    static double Divide(double x, double y) => x / y;
}
