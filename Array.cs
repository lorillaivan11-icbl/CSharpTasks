namespace ConsoleApp1;
public class Task2
{
static void Main(string[] args)
    {
// Row 0: Even numbers
// Row 1: Odd numbers
int[][] numberMatrix = new int[][]
{
    new int[] { 2, 4, 6, 8, 10 },
    new int[] { 1, 3, 5, 7, 9 }
};

Console.WriteLine("The number matrix has been initialized.");

{   //digit 1
    int digit1 = numberMatrix[1][3];
Console.WriteLine("Output");
    Console.WriteLine($"Digit 1 is: {digit1}");
    
    //digit 2 
    int digit2 = numberMatrix[0][0];
    Console.WriteLine($"Digit 2 is: {digit2}");

    //digit 3
    int digit3 = numberMatrix[1][4];
    Console.WriteLine($"Digit 3 is: {digit3}");

    //combining the digits using the key
    string thekey = string.Format("{0}{1}{2}", digit1, digit2, digit3);
    Console.WriteLine($"the password is: {thekey}");
}

    }
}
