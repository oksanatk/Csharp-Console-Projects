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
ShowMainMenu();

List<Game> games = new List<Game>();
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
        Console.WriteLine("[bold yellow]Welcome to our little Math Game![/]");
        Console.WriteLine("\nYour menu options are:");
        Console.WriteLine("\r1. View previous games");
        Console.WriteLine("\r2. Start a new game");
        Console.WriteLine("\nOR type 'exit' to exit the game");

        readResult = Console.ReadLine();
        if (readResult != null)
        {
            userMenuChoice = readResult.Trim().ToLower();
        }
        switch (userMenuChoice)
        {
            case "exit":
                Console.WriteLine("You are choosing to exit the game. Goodbye!");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                break;

            case "one":
            case "1":
                Console.WriteLine("You are choosing to view the previous games.");
                Console.Write("Press any key to continue");
                Console.ReadKey();
                ShowPreviousGames(games);
                break;

            case "two":
            case "2":
                Console.WriteLine("You are choosing to play a new game");
                // call GetNewGameConditions() to validate which level / difficulty they would like
                // then call PlayNewGame(Operator operation, Difficulty level) to play / store info of game
        }

    } while (userMenuChoice != "exit");


}

void ShowPreviousGames(List<Game> previousGames)
{

}

record Game (List<MathProblem> problems, List<Operator> operations, Difficulty level, bool winOrLose, string timeToPlay);
record MathProblem (int num1, int num2, Operator operation, int userAnswer, bool correct);

enum Operator
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Random
}

enum Difficulty
{
    Easy,
    Medium,
    Hard
}

