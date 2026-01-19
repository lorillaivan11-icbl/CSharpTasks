namespace ConsoleApp1;
public class Task5
{
    static void Main()
    {
        Console.Write("Enter Total Students: ");
        int totalStudents;

        // Validate total students input
        while (!int.TryParse(Console.ReadLine(), out totalStudents) || totalStudents <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive number.");
            Console.Write("Enter Total Students: ");
        }

        // Multi-dimensional array to store student data
        // Columns: 0 - Name, 1 - English Marks, 2 - Math Marks, 3 - Computer Marks, 4 - Total Marks
        string[,] students = new string[totalStudents, 5];

        // Input data for each student
        for (int i = 0; i < totalStudents; i++)
        {
            Console.WriteLine($"\nEnter details for Student {i + 1}:");

            // Input student name
            Console.Write("Enter Student Name: ");
            students[i, 0] = Console.ReadLine();

            // Input marks for English, Math, and Computer
            students[i, 1] = GetValidatedMarks("English");
            students[i, 2] = GetValidatedMarks("Math");
            students[i, 3] = GetValidatedMarks("Computer");

            // Calculate total marks
            int totalMarks = int.Parse(students[i, 1]) + int.Parse(students[i, 2]) + int.Parse(students[i, 3]);
            students[i, 4] = totalMarks.ToString();
        }

        // Sort students by total marks in descending order
        SortStudentsByTotalMarks(students);

        // Display the report card
        Console.WriteLine("\n**************** Report Card ****************\n");

        for (int i = 0; i < totalStudents; i++)
        {
            Console.WriteLine($"************************************");
            Console.WriteLine($"Student Name: {students[i, 0]}, Position: {i + 1}, Total: {students[i, 4]}/300");
        }

        Console.WriteLine("************************************");
    }

    // Method to get validated marks for a subject
    static string GetValidatedMarks(string subjectName)
    {
        int marks;
        Console.Write($"Enter {subjectName} Marks (Out Of 100): ");

        // Validate marks input
        while (!int.TryParse(Console.ReadLine(), out marks) || marks < 0 || marks > 100)
        {
            Console.WriteLine("Invalid input. Please enter a number between 0 and 100.");
            Console.Write($"Enter {subjectName} Marks (Out Of 100): ");
        }

        return marks.ToString();
    }

    // Method to sort students by total marks in descending order
    static void SortStudentsByTotalMarks(string[,] students)
    {
        int totalStudents = students.GetLength(0);

        for (int i = 0; i < totalStudents - 1; i++)
        {
            for (int j = i + 1; j < totalStudents; j++)
            {
                // Compare total marks (column 4)
                if (int.Parse(students[i, 4]) < int.Parse(students[j, 4]))
                {
                    // Swap rows
                    for (int k = 0; k < 5; k++)
                    {
                        string temp = students[i, k];
                        students[i, k] = students[j, k];
                        students[j, k] = temp;
                    }
                }
            }
        }
    }
}
