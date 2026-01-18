using System;

namespace ConsoleApp1
{
    public class Task3
    {
        public static void Main(string[] args)
        {
            int[] numbers = { 3, 7, 12, 19, 21, 25, 30 };

            // The 'while' loop keeps the program running
            while (true)
            {
                Console.WriteLine("\n--- New Search ---");
                Console.Write("Enter a number to search for (or type 'exit' to quit): ");
                string input = Console.ReadLine();

                // Check if the user wants to stop the entire program
                if (input.ToLower() == "exit")
                {
                    Console.WriteLine("Goodbye!");
                    break; // This break exits the WHILE loop
                }

                if (int.TryParse(input, out int searchTarget))
                {
                    bool isFound = false;

                    // The 'for' loop for searching
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        if (numbers[i] == searchTarget)
                        {
                            Console.WriteLine($"Number found at position {i}!");
                            isFound = true;
                            break; // This break exits the FOR loop only
                        }
                    }

                    if (!isFound)
                    {
                        Console.WriteLine("Number not found in the list.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }
        }
    }
}
