float num1=0;
float num2=0;
string? readResult;


ShowMainMenu();

void ShowMainMenu()
{
    bool validNum1 = false;
    do
    {
        // outer loop continues until num1 is assigned a real number

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

        bool validNum2 = false;
        do
        {
            // inner loop continues while num2 isn't a real number

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
    } while(!validNum1);

    Console.WriteLine("Choose an option from the following list:");
    Console.WriteLine("\ta - Add");
    Console.WriteLine("\ts - Subtract");
    Console.WriteLine("\tm - Multiply");
    Console.WriteLine("\td - Divide");
    Console.Write("Your option? ");

    // Use a switch statement to do the math.
    switch (Console.ReadLine())
    {
        case "a":
            Console.WriteLine($"Your result: {num1} + {num2} = " + (num1 + num2));
            break;
        case "s":
            Console.WriteLine($"Your result: {num1} - {num2} = " + (num1 - num2));
            break;
        case "m":
            Console.WriteLine($"Your result: {num1} * {num2} = " + (num1 * num2));
            break;
        case "d":
            while (num2 == 0)
            {
                Console.WriteLine("Can't divide by 0. Please enter a different number.");
                num2 = Convert.ToInt32(Console.ReadLine());
            }
            Console.WriteLine($"Your result: {num1} / {num2} = " + (num1 / num2));
            break;
        default:
            Console.WriteLine("I didn't understand the option that you wanted.");
            break;
    }
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

