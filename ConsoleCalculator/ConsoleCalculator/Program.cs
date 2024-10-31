using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

float num1=0;
float num2=0;
string? readResult;


ShowMainMenu();

void ShowMainMenu()
{
    bool endApp = false;
    bool validNum1 = false;
    do
    {
        Console.Clear();
        Console.WriteLine("Console Calculator in C#");
        Console.WriteLine("------------------------\n");

        Console.WriteLine("\nEnter a number, and then press the 'Enter' key to continue.");
        readResult = Console.ReadLine();
        if (readResult != null)
        {
            if (float.TryParse(readResult, out num1))
            {
                validNum1 = true;
            }
            else
            {
                Console.WriteLine("Sorry, but I couldn't understand that number. Press the 'Enter' key to try again.");
                Console.ReadLine();
            }
        }
    } while(!validNum1);

    bool validNum2 = false;
    do
    { 
        Console.WriteLine("\nEnter another number, and then press the 'Enter' key to continue.");

        readResult = Console.ReadLine();
        if (readResult != null)
        {
            if (float.TryParse(readResult, out num2))
            {
                validNum2 = true;   
            }
            else
            {
                Console.WriteLine("Sorry, but I couldn't understand that number. Press the 'Enter' key to try again.");
                Console.ReadLine();
            }
        }
    } while(!validNum2);

    string? op;
    double result = 0;

    Console.WriteLine("Choose an option from the following list:");
    Console.WriteLine("\ta - Add");
    Console.WriteLine("\ts - Subtract");
    Console.WriteLine("\tm - Multiply");
    Console.WriteLine("\td - Divide");
    Console.Write("Your option? ");

    op = Console.ReadLine();
    if (readResult != null || !(Regex.IsMatch(readResult,"[a|s|m|d]")))
    {
        Console.WriteLine("I'm sorry, but I didn't understand that operator.");
    }else
    {
        try
        {
            result = Calculator.DoOperation(num1, num2, readResult);
            if (double.IsNaN(result))
            {
                Console.WriteLine("This operation results in a mathematical error.");
            }
            else Console.WriteLine("Your result {0:0.##}\n", result);     
        }
        catch (Exception e)
        {
            Console.WriteLine("Oh no! An exception by the name of 'E' occured while we were trying to do the math.\n - Details: " + e.Message);
        }
    }
    Console.WriteLine("------------------------\n");

    Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue.");
    if (Console.ReadLine() == "n") endApp = true;
            
    Console.WriteLine("\n");
}

class Calculator
{
    public static double DoOperation(double num1, double num2, string op)
    {
        double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.

        switch (op)
        {
            case "a":
                result = num1 + num2;
                break;
            case "s":
                result = num1 - num2;
                break;
            case "m":
                result = num1 * num2;
                break;
            case "d":
                while (num2 == 0)
                {
                    Console.WriteLine("Cannot divide by 0. Please enter another number.");
                    num2 = Convert.ToDouble(Console.ReadLine());
                }
                if (num2 != 0)
                {
                    result = num1 / num2;
                }
                break;

            default:
                Console.WriteLine("I'm sorry, I didn't understand that operation.");
                break;
        }
        return result;
    }
}

