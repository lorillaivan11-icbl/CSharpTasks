using System;

namespace CalculatorApp
{
    class Task4
    {
        static void Main(string[] args)
        {
            char continueChoice = 'Y';

            // While loop to repeat the program until user presses 'N'
            while (char.ToUpper(continueChoice) != 'N')
            {
                Console.WriteLine("Press any following key to perform an arithmetic operation:");
                Console.WriteLine("1 - Addition");
                Console.WriteLine("2 - Subtraction");
                Console.WriteLine("3 - Multiplication");
                Console.WriteLine("4 - Division");

                string choice = Console.ReadLine();

                Console.Write("Enter Value 1: ");
                double val1 = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter Value 2: ");
                double val2 = Convert.ToDouble(Console.ReadLine());

                // Switch-case structure to route to correct methods
                switch (choice)
                {
                    case "1":
                        Console.WriteLine($"{val1} + {val2} = {Add(val1, val2)}");
                        break;
                    case "2":
                        Console.WriteLine($"{val1} - {val2} = {Subtract(val1, val2)}");
                        break;
                    case "3":
                        Console.WriteLine($"{val1} * {val2} = {Multiply(val1, val2)}");
                        break;
                    case "4":
                        if (val2 != 0)
                            Console.WriteLine($"{val1} / {val2} = {Divide(val1, val2)}");
                        else
                            Console.WriteLine("Error: Cannot divide by zero.");
                        break;
                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }

                Console.Write("Do you want to continue again (Y/N)? ");
                continueChoice = Console.ReadKey().KeyChar;
                Console.WriteLine(); // Line break for formatting
            }
        }

        // Dedicated Methods for each operation
        static double Add(double a, double b) => a + b;
        static double Subtract(double a, double b) => a - b;
        static double Multiply(double a, double b) => a * b;
        static double Divide(double a, double b) => a / b;
    }
}
