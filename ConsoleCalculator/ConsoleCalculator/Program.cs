using System.Text.RegularExpressions;
using CalculatorLibrary;

float num1=0;
float num2=0;
string? readResult;
Calculator calculator = new Calculator();

ShowMainMenu();

void ShowMainMenu()
{

    bool endApp = false;

    while(!endApp)
    {
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
        } while (!validNum1);

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
        } while (!validNum2);

        string? op;
        double result = 0;

        Console.WriteLine("Choose an option from the following list:");
        Console.WriteLine("\ta - Add");
        Console.WriteLine("\ts - Subtract");
        Console.WriteLine("\tm - Multiply");
        Console.WriteLine("\td - Divide");
        Console.Write("Your option? ");

        op = Console.ReadLine();
        if (op == null || !(Regex.IsMatch(op, "[a|s|m|d]")))
        {
            Console.WriteLine("I'm sorry, but I didn't understand that operator.");
        }
        else
        {
            try
            {
                result = calculator.DoOperation(num1, num2, op);
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

        Console.Write("Enter 'exit' and Enter to close the app, or press any other key and Enter to continue.");
        if (Console.ReadLine() == "exit") endApp = true;

        Console.WriteLine("\n");
    }        
    calculator.Finish();
}



