using Spectre.Console;
using System.Diagnostics;

/*  Math Game challenge by the C# Academy
 *  Criteria:
 *  -- a win / lose condition , i guess?
 *  -- use all 4 basic operation (+,-, * , /)
 *     -- division presented to user should only result in integers
 *  -- users should have a menu option to choose the operation they are using
 *  -- record previous games in a List, with an option for user to VISUALIZE previous games
 *  
 *  Extra Bonus Points:
 *  - different levels of difficulty
 *  - a timer that counts how long each game lasted (stopwatch)
 *  - random game option that presents users with a game that can have any operation
 *  
 *  - speech recognition from user
 *  
 *  Plan: 
 *  1st try: code what I think it should be. Don't worry about perfection yet, just make sure it works.
 *  2nd go-around: watch the tutorial and re-factor my previous code to be more readable / less repetitive
 *  */

// okay -> how does my game work? Do they have to get 10 operations correct in a row to win? + any wrong answer = a loss?
List<Game> games = new List<Game>();
Random random = new Random();

ShowMainMenu();
void ShowMainMenu()
{
    // Display welcome banner (how to modify text size with Spectre.Console?
    // Display number of previous games as a number + menu option to view previous games
    //      + menu option to exit
    //      + menu option to play a new game     --> call CreateNewGame()?

    // Choose the Operation that you'd like to play + validate input

    // Choose the difficulty + validate input

    // Call PlayNewGame(userOperation, userDifficulty)
    string? readResult = "";
    string userMenuChoice = "";

    do
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[bold yellow]Welcome to our little Math Game![/]");
        AnsiConsole.MarkupLine("\nYour menu options are:");
        AnsiConsole.MarkupLine("\r1. View previous games");
        AnsiConsole.MarkupLine("\r2. Start a new game");
        AnsiConsole.MarkupLine("\nOR type 'exit' to exit the game");

        readResult = Console.ReadLine();
        if (readResult != null)
        {
            userMenuChoice = readResult.Trim().ToLower();
        }
        switch (userMenuChoice)
        {
            case "exit":
                AnsiConsole.MarkupLine("You are choosing to exit the game. Goodbye!");
                AnsiConsole.MarkupLine("Press any key to continue");
                Console.ReadKey();
                break;

            case "one":
            case "1":
                AnsiConsole.MarkupLine("You are choosing to view the previous games.");
                Console.Write("Press any key to continue");
                Console.ReadKey();
                ShowPreviousGames(games);
                break;

            case "two":
            case "2":
                AnsiConsole.MarkupLine("You are choosing to play a new game");
                Operator operation;
                Difficulty level;

                GetNewGameConditions(out level, out operation);
                games.Add(PlayNewGame(operation,level));
                break;
            // call GetNewGameConditions() to validate which level / difficulty they would like
            // then call PlayNewGame(Operator operation, Difficulty level) to play / store info of game

            default:
                AnsiConsole.MarkupLine("Sorry, but we didn't recognize that menu option.");
                AnsiConsole.MarkupLine("Please press Enter to try again.");
                Console.ReadLine();
                break;
        }

    } while (userMenuChoice != "exit");

}

void GetNewGameConditions(out Difficulty level, out Operator op)
{
    op = Operator.addition;  // operator assignment necessary to avoid Error CS0177
                             // compiler? thinks that the else statment in the outside loop is a branch that will never assign op, which is false
                             // because the while loop will repeat forever
    string? readResult;
    string userOperationChoice = "";
    string userDifficultyChoice = "";
    bool validOperation = false;
    bool validDifficulty = false;

    do
    {
        Console.Clear();
        AnsiConsole.MarkupLine("Before we begin, we'd like to ask you about the type of game you'd like to play.");
        AnsiConsole.MarkupLine("\nHow difficult would you like your game to be?");
        AnsiConsole.MarkupLine("The options are: [bold yellow]easy[/], [bold yellow]medium[/], and [bold yellow]hard[/]");

        readResult = Console.ReadLine();
        if (readResult != null)
        {
            userDifficultyChoice = readResult.Trim().ToLower();
        }

        if (Enum.TryParse<Difficulty>(userDifficultyChoice, out level))
        {
            AnsiConsole.MarkupLine($"\nWe've recorded that you want to play a(n) [bold aqua]{level}[/] game.");

            do
            {
                AnsiConsole.MarkupLine("\nGreat! Can you also tell us which operation you'd like to play the game in?");
                AnsiConsole.MarkupLine("\nYour options are:[bold yellow] addition[/], [bold yellow]subtraction[/], [bold yellow]multiplication[/], [bold yellow]division[/], or [bold yellow]random[/].");
                AnsiConsole.MarkupLine("Random means that you may get problems in any operator.");

                readResult = Console.ReadLine();
                if (readResult != null)
                {
                    userOperationChoice = readResult.Trim().ToLower();
                }

                if (Enum.TryParse<Operator>(userOperationChoice, out op))
                {
                    AnsiConsole.MarkupLine($"\nWe've recorded that you want to play a game using the [bold aqua]{op}[/] operation.");
                    validOperation = true;
                } else
                {
                    AnsiConsole.MarkupLine("We're sorry, but we couldn't recognize that operation type. Please try again.");
                }

                AnsiConsole.MarkupLine("Press the Enter key to continue");
                Console.ReadLine();
            } while (!validOperation);

            validDifficulty = true;
        }else
        {
            AnsiConsole.MarkupLine("We're sorry, but we couldn't recognize that level of difficulty.");
            AnsiConsole.MarkupLine("Press the Enter key to continue.");
            Console.ReadLine();
        } 
    } while(!validDifficulty); 
}

Game PlayNewGame(Operator currentOperator, Difficulty level)
{
    List<MathProblem> currentProblems = new List<MathProblem>();
    List<Operator> currentOperations = new List<Operator>();
    bool ifWin = false;
    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
    stopwatch.Start();

    // game code here
    AnsiConsole.MarkupLine($"For testing purposes, you are currently playing a game with {currentOperator} operations and {level} difficulty.");
    AnsiConsole.Confirm("testing the ansiconsole confirm");


    stopwatch.Stop();
    string timeElapsed = stopwatch.ToString();
    Game thisGame = new Game(currentProblems, currentOperations, level, ifWin, timeElapsed);

    return thisGame;
}

void ShowPreviousGames(List<Game> previousGames)
{

}

record Game (List<MathProblem> Problems, List<Operator> Operations, Difficulty Level, bool WinOrLose, string TimeToPlay);
record MathProblem (int Num1, int Num2, Operator Operation, int UserAnswer, bool Correct);

enum Operator
{
    addition,
    subtraction,
    multiplication,
    division,
    random
}

enum Difficulty
{
    easy,
    medium,
    hard
}

