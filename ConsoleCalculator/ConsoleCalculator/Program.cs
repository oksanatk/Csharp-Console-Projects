using Spectre.Console;
using System.Text.RegularExpressions;
using CalculatorLibrary;

string? readResult;
Calculator calculator = new Calculator();

ShowMainMenu();

void ShowMainMenu()
{
    bool endApp = false;

    while (!endApp)
    {
        double num1 = double.NaN;
        double num2 = double.NaN;
        string op = "";
        double result = double.NaN;

        do
        {
            Console.Clear();
            Console.WriteLine("Console Calculator in C#");
            Console.WriteLine("------------------------\n");

            Console.WriteLine("Choose an operation from the following list:\n");
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
            Console.WriteLine("Enter 'calc' to see the previous calculations");
            Console.WriteLine("Enter 'exit' to exit the app.");
            Console.Write("\nYour option? ");





            readResult = Console.ReadLine();
            if (readResult != null)
            {
                op = readResult.Trim().ToLower();
            }

            if (op == "exit") { endApp = true; }

            else if (op == "num") { Console.WriteLine("The calculator has been used {0} times.", calculator.TimesUsed); }

            else if (op == "calc")
            {
                Console.Clear();
                Console.WriteLine("You are choosing to view previous operations.\n");
                calculator.ShowPreviousCalculations(calculator.RecentCalculations);
                Console.WriteLine("\nEnter 'clear' to clear the history of recent calculations, or just press the 'Enter' key to continue.");

                readResult = Console.ReadLine();
                if (readResult != null)
                {
                    string userInput = readResult.Trim().ToLower();
                    if (userInput.StartsWith("c"))
                    {
                        calculator.ClearCalculations();
                    }
                }
            }
            else if (Regex.IsMatch(op, "(add)|(sub)|(mult)|(div)|(sq)|(pow)|(10x)|(sin)|(cos)|(tan)"))
            {
                // valid operator, call function?
                Console.WriteLine("You're choosing to {0} operation.", op);
            }
            else
            {
                Console.WriteLine("I'm sorry, but I didn't understand that operator. Press Enter to try again.");
                Console.ReadLine();
            }
        } while (!Regex.IsMatch(op, "(add)|(sub)|(mult)|(div)|(sq)|(pow)|(10x)|(sin)|(cos)|(tan)|(num)|(exit)"));

        if (op != "num" && op != "exit")
        {
            num1 = GetNum();

            // if not single-num operation, get num2
            if (op != null && Regex.IsMatch(op, "(add)|(sub)|(mult)|(div)|(pow)"))
            {
                num2 = GetNum();
            }
        }

        if (op != "exit" && op != "num" && op != "calc" && op != null)
        {
            result = calculator.DoOperation(num1, num2, op);
            if (double.IsNaN(result))
            {
                Console.WriteLine("This operation results in a mathematical error.");
            }
            else Console.WriteLine("Your result is: {0:0.##}\n", result);

            Console.WriteLine("------------------------\n");
            if (op != "exit")
            {
                Console.Write("Enter 'exit' to close the app, or press the 'Enter' key to continue. ");
                if (Console.ReadLine() == "exit") endApp = true;

                Console.WriteLine("\n");
            }
        }
    }
    calculator.Finish();
}


double GetNum()
{
    double userNum = double.NaN;
    bool validNum = false;
    do
    {
        Console.WriteLine("\nEnter a number, and then press the 'Enter' key to continue.");
        Console.WriteLine("OR enter 'calc' to view the previous calculations and results.");
        readResult = Console.ReadLine();
        if (readResult != null)
        {
            if (readResult.StartsWith("calc"))
            {
                calculator.ShowPreviousCalculations(calculator.RecentCalculations);
                Console.WriteLine("That is the end of the recent calculations.");
                Console.WriteLine("\t---------------------------\n");

            }
            else if (double.TryParse(readResult, out userNum))
            {
                validNum = true;
            }
            else
            {
                Console.WriteLine("Sorry, but I couldn't understand that number. Press the 'Enter' key to try again.");
                Console.ReadLine();
            }
        }
    } while (!validNum);
    return userNum;
}


