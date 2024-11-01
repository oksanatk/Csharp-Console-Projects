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

    while (!endApp)
    {
        string op = "";
        double result = 0;

        do
        {
            Console.Clear();
            Console.WriteLine("Console Calculator in C#");
            Console.WriteLine("------------------------\n");

            Console.WriteLine("Choose an operation from the following list:");
            Console.WriteLine("\tadd - Add");
            Console.WriteLine("\tsub - Subtract");
            Console.WriteLine("\tmult - Multiply");
            Console.WriteLine("\tdiv - Divide");
            Console.WriteLine("\tsq - Square Root of a Number");
            Console.WriteLine("\tpow - Raise to the Power");
            Console.WriteLine("\t10x - 10");
            Console.WriteLine("\tsin - Sin of a Number");
            Console.WriteLine("\tcos - Cos of a Number");
            Console.WriteLine("\ttan - Tangent of a Number");

            Console.WriteLine("OR \nEnter 'num' to see the number of times the calculator has been used.");
            Console.WriteLine("Enter 'exit' to exit the app.");
            Console.Write("Your option? ");

            readResult = Console.ReadLine();
            if (readResult != null)
            {
                op = readResult.Trim().ToLower();
            }

            if (!Regex.IsMatch(op, "[add|sub|mult|div|sq|pow|10x|sin|cos|tan]")) 
            {
                if (op.Trim().ToLower() == "exit")
                {
                    endApp = true;
                    break;
                }
                else if (op == "num")
                {
                    Console.WriteLine(calculator.TimesUsed);
                }
                else
                {
                    Console.WriteLine("I'm sorry, but I didn't understand that operator.");
                } 
            }
        } while (!Regex.IsMatch(op, "[add|sub|mult|div|sq|pow|10x|sin|cos|tan]"));

        // get num1
        bool validNum1 = false;
        do
        {
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

        // if not single-num operation, get num2
        if (op != null && !Regex.IsMatch(op, "[sq|10x|sin|cos|tan]"))
        {        
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
        }

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

        Console.WriteLine("------------------------\n");

        Console.Write("Enter 'exit' to close the app, or press the 'Enter' key to continue.");
        if (Console.ReadLine() == "exit") endApp = true;

        Console.WriteLine("\n");
    }        
    calculator.Finish();
}



