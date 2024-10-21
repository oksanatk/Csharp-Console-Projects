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

Game PlayNewGame(Operator originalOperator, Difficulty level)
{
    Operator currentOperator = originalOperator;
    List<MathProblem> currentProblems = new List<MathProblem>();

    bool didUserWin = false;
    int maxNum = 1;
    int num1;
    int num2;
    int divisionProduct;
    int userInt = -101;
    bool isCorrect = false;
    string prompt;
    int correctAnswerCounter = 0;

    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
    stopwatch.Start();

    switch (level)
    {
        case Difficulty.easy:
            maxNum = 11;
            break;
        case Difficulty.medium:
            maxNum = 51;
            break;
        case Difficulty.hard:
            maxNum = 101;
            break;
    }

    // for each loop: display progress, previous problems in game, and collect user answer. then, continue.
    for (int i = 0; i < 10; i++)
    {
        num1 = random.Next(1, maxNum);
        num2 = random.Next(1, maxNum);

        Console.Clear();
        AnsiConsole.Progress()
                .HideCompleted(false)
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                })
                .Start(ctx =>
                {
                    var taskAnswerProblems = ctx.AddTask("[green]Problems Answered in this Game:[/]");
                    taskAnswerProblems.Increment(i * 10);
                });

        DisplayPreviousProblems(currentProblems, true);

        if (originalOperator == Operator.random)
        {
            int randOperator = random.Next(0, 4);
            currentOperator = (Operator)randOperator;
        }

        switch (currentOperator)
        {
            case Operator.addition:
                prompt = $"{num1} + {num2} = ";
                userInt = GetUserAnswer(prompt);
                isCorrect = num1 + num2 == userInt ? true : false;
                correctAnswerCounter = num1 + num2 == userInt ? correctAnswerCounter += 1 : correctAnswerCounter += 0;
                break;

            case Operator.subtraction:
                prompt = $"{num1} - {num2} = ";
                userInt = GetUserAnswer(prompt);
                isCorrect = num1 - num2 == userInt ? true : false;
                correctAnswerCounter = num1 - num2 == userInt ? correctAnswerCounter += 1 : correctAnswerCounter += 0;
                break;

            case Operator.multiplication:
                prompt = $"{num1} * {num2} = ";
                userInt = GetUserAnswer(prompt);
                isCorrect = num1 * num2 == userInt ? true : false;
                correctAnswerCounter = num1 * num2 == userInt ? correctAnswerCounter += 1 : correctAnswerCounter += 0;
                break;

            case Operator.division:
                divisionProduct = num1 * num2;
                num2 = num1;
                num1 = divisionProduct;

                prompt = $"{num1} / {num2} = ";
                userInt = GetUserAnswer(prompt);
                isCorrect = num1 / num2 == userInt ? true : false;
                correctAnswerCounter = num1 / num2 == userInt ? correctAnswerCounter += 1 : correctAnswerCounter += 0;
                break;
        }
        currentProblems.Add(new MathProblem(num1, num2, currentOperator, userInt, isCorrect));
    }
    didUserWin = correctAnswerCounter >= 8 ? true : false;

    stopwatch.Stop();
    string[] bigTimes = stopwatch.ToString().Split('.');
    string[] minSecTimes = bigTimes[0].Split(":");
    string timeElapsed = $"{minSecTimes[1]} minutes and {minSecTimes[2]} seconds";

    // final display of the problems and answers of completed game below
    DisplayPreviousProblems(currentProblems, true);

    AnsiConsole.MarkupLine($"\nDun dun dunnnnnn....... \nDid you win or lose? \n{(didUserWin ? "[green]You won![/]" : "[maroon]You lost![/]")}");
    AnsiConsole.MarkupLine($"You got {correctAnswerCounter} / 10 questions right.");
    AnsiConsole.MarkupLine($"It took you: {timeElapsed} to complete the game.");

    AnsiConsole.MarkupLine("\n\nPress any key to continue.");
    Console.ReadKey();

    Game thisGame = new Game(currentProblems, level, didUserWin, timeElapsed);
    return thisGame;
}

void DisplayPreviousProblems(List<MathProblem> problems, bool showProgress)
{
    string viewProblem = $"";
    string userAnswerColor;

    if (showProgress)
    {    
        Console.Clear();
        AnsiConsole.Progress()
                .HideCompleted(false)
                .Columns(new ProgressColumn[]
                {
                        new TaskDescriptionColumn(),
                        new ProgressBarColumn(),
                        new PercentageColumn(),
                })
                .Start(ctx =>
                {
                    var taskAnswerProblems = ctx.AddTask("[green]Problems Answered in this Game:[/]");
                    taskAnswerProblems.Increment(problems.Count * 10);
                });
    }

    foreach (MathProblem p in problems)
    {
        userAnswerColor = p.Correct ? $"[green]{p.UserAnswer}[/]" : $"[maroon]{p.UserAnswer}[/]";
        viewProblem = $"[bold yellow]{p.Num1}[/] {GetOperatorString(p.Operation)} [bold yellow]{p.Num2}[/] = " + userAnswerColor;

        AnsiConsole.MarkupLine(viewProblem);
    }
}
int GetUserAnswer(string prompt)
{
    int userNum = -101;
    string? readResult;
    bool validAnswer = false;


    while (!validAnswer)
    {
        AnsiConsole.Markup(prompt);
        readResult = Console.ReadLine();
        if (readResult != null)
        {
            if (int.TryParse(readResult, out userNum))
            {
                validAnswer = true;
            }
            else
            {
                AnsiConsole.MarkupLine("\nSorry, but that didn't look like a number. Please try again.");
            }
        } 
    }

    return userNum;
}

void ShowPreviousGames(List<Game> previousGames)
{
    Console.Clear();
    if(previousGames.Count == 0)
    {
        AnsiConsole.MarkupLine("There are no previous games to display.");
    }
    else
    {
        Table table = new Table().Centered();
        table.AddColumn("Problem:");
        table.AddColumn("Answer");

        foreach (Game game in previousGames)
        {
            foreach (MathProblem problem in game.Problems)
            {
                string userAnswerColor = problem.Correct ? $"[green]{problem.UserAnswer}[/]" : $"[maroon]{problem.UserAnswer}[/]";
                table.AddRow($"[bold yellow]{problem.Num1}[/] {GetOperatorString(problem.Operation)} [bold yellow]{problem.Num2}[/] = ", userAnswerColor);
            }
            table.AddEmptyRow();
            table.AddRow("Winner?", (game.WinOrLose ? "[green]Winner![/]" : "[maroon]Loser.[/]"));
            table.AddRow("Time to Play:", game.TimeToPlay);
        }
        AnsiConsole.Write(table); 
    }

    AnsiConsole.MarkupLine("\nPress the Enter key to continue.");
    Console.ReadLine();

}
string GetOperatorString(Operator operation)
{
    switch (operation)
    {
        case Operator.addition:
            return "+";
        case Operator.subtraction:
            return "-";
        case Operator.division:
            return "/";
        case Operator.multiplication:
            return "*";
    }

    return "";
}

record Game (List<MathProblem> Problems, Difficulty Level, bool WinOrLose, string TimeToPlay);
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

