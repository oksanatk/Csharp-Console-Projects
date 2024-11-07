using Spectre.Console;
using System.Text.RegularExpressions;
using CalculatorLibrary;

string? readResult;
//string[] validMenuChoices = new string[] { "add", "sub", "mult", "div", "sq", "pow", "10x", "sin", "cos", "tan", "exit"};
string[] validOperations = new string[] { "add", "sub", "mult", "div", "sq", "pow", "10x", "sin", "cos", "tan" };
string[] singleNumberOperations = new string[] { "sq", "10x", "sin", "cos", "tan" }; 
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

            else if (op == "num") 
            { 
                Console.WriteLine("\nThe calculator has been used {0} times.", calculator.TimesUsed);
                Console.WriteLine("\nPress the 'Enter' key to continue back to the main menu.");
                Console.ReadLine();
            }

            else if (op == "calc")
            {
                Console.Clear();
                Console.WriteLine("You are choosing to view previous operations.");
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
            else if (validOperations.Contains(op))
            {
                // valid operator, call function?
                Console.WriteLine("You're choosing to {0} operation.", op);
            }
            else
            {
                Console.WriteLine("I'm sorry, but I didn't understand that operator. Press Enter to try again.");
                Console.ReadLine();
            }
        } while (!validOperations.Contains(op) && op !="exit");

        if (op != "exit" && op !=null)
        {
            num1 = GetNum(false);

            // if not single-num operation, get num2
            if (!singleNumberOperations.Contains(op))
            {
                num2 = GetNum(true);
            }

            result = calculator.DoOperation(num1, num2, op);
            if (double.IsNaN(result))
            {
                Console.WriteLine("This operation results in a mathematical error.");
            }
            else Console.WriteLine("Your result is: {0}\n", result);

            Console.WriteLine("\t------------------------\n");
            Console.Write("Enter 'exit' to close the app, or press the 'Enter' key to continue. ");

            if (Console.ReadLine() == "exit") endApp = true;

            Console.WriteLine("\n");      
        }
    }
    calculator.Finish();
}


double GetNum(bool gettingSecondNum)
{
    double userNum = double.NaN;
    bool validNum = false;
    do
    {
        if (gettingSecondNum)
        {
            Console.WriteLine("\nPlease enter a second number.");
        }else
        {
            Console.WriteLine("\nPlease enter a number.");

        }
        Console.WriteLine("OR enter 'calc' to view the previous calculations and results.");
        readResult = Console.ReadLine();
        if (readResult != null)
        {
            if (readResult.StartsWith("calc"))
            {
                calculator.ShowPreviousCalculations(calculator.RecentCalculations);
                Console.WriteLine("\n\t---------------------------\n");

                Console.WriteLine("Type 'calculation #' to use the result of a previous calculation. IE, 'calculation 1'");
                Console.WriteLine("OR enter any number (##) to continue as usual.");

                readResult = Console.ReadLine();
                if (readResult != null && readResult.StartsWith("calc"))
                {
                    string[] parseCalculationNumber = readResult.Split(' ');
                    int previousCalculation = 0;
                    if (int.TryParse(parseCalculationNumber[1],out previousCalculation))
                    {
                        if ((previousCalculation > 0) && (previousCalculation <= calculator.RecentCalculations.Count))
                        {
                            validNum = true;
                            userNum = calculator.RecentCalculations[(previousCalculation - 1)].Result;
                            Console.WriteLine($"Recording your number as {userNum}");

                        } else
                        {
                            Console.WriteLine("Sorry, but we couldn't find a corresponding calculation result.");
                        }
                    } else
                    {
                        Console.WriteLine("Sorry, but I couldn't understand that number. Press the 'Enter' key to try again.");
                    }
                }
            }
            
            if (double.TryParse(readResult, out userNum))
            {
                validNum = true;
            }
            else if (readResult != null && !readResult.StartsWith("calc"))
            {
                Console.WriteLine("Sorry, but I couldn't understand that number. Press the 'Enter' key to try again.");
                Console.ReadLine();
            }
        }
    } while (!validNum);
    return userNum;
}


