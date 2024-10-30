using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Diagnostics.Logging;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

//  Math Game challenge to meet criteria by the C# Academy
//  Note: Speech Recognition mode is only available by running with command-line argument --voice-input

string? speechKey = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Key");
string? speechRegion = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Region");
var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
speechConfig.SpeechRecognitionLanguage = "en-US";

List<Game> games = new List<Game>();
Random random = new Random();
bool speechInputMode = false;

// sample games for testing purposes below
/*
Game test1 = new Game(
    new List<MathProblem> {
        new MathProblem(1, 2, Operator.addition, 5, false),
        new MathProblem(1, 2, Operator.addition, 5, false),
        new MathProblem(1, 2, Operator.addition, 5, false),
        new MathProblem(1, 2, Operator.addition, 5, false),
        new MathProblem(1, 2, Operator.addition, 5, false),
        new MathProblem(1, 2, Operator.addition, 5, false),
        new MathProblem(1, 2, Operator.addition, 5, false),
    },
    Difficulty.easy,
    false,
    "XX minutes and XX seconds"
    );

Game test2 = new Game(
    new List<MathProblem> {
        new MathProblem(1, 2, Operator.addition, 3, true),
        new MathProblem(1, 2, Operator.addition, 3, true),
        new MathProblem(1, 2, Operator.addition, 3, true),
        new MathProblem(1, 2, Operator.addition, 3, true),
        new MathProblem(1, 2, Operator.addition, 3, true),
        new MathProblem(1, 2, Operator.addition, 3, true),
        new MathProblem(1, 2, Operator.addition, 3, true),
    },
    Difficulty.easy,
    true,
    "YY minutes and YY seconds"
    );

games.Add(test1);
games.Add(test2);
*/

if (args.Contains("--voice-input"))
{
    AnsiConsole.MarkupLine("[bold blue]\nVoice Input Chosen![/]");
    speechInputMode = true;
}

await ShowMainMenu(speechInputMode);

async Task ShowMainMenu(bool voiceMode)
{
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

        if (voiceMode)
        {
            readResult = await GetVoiceInput();
        } else
        {
            readResult = Console.ReadLine();
        } 

        if (readResult != null)
        {
            userMenuChoice = readResult.Trim().ToLower();
        }
        switch (userMenuChoice)
        {
            case "exit":
                AnsiConsole.MarkupLine("You are choosing to exit the game. Goodbye!");
                if (voiceMode)
                {
                    await Task.Delay(3000);
                } else
                {
                    AnsiConsole.MarkupLine("Press any key to continue");
                    Console.ReadKey();
                }
                break;

            case "one":
            case "1":
                AnsiConsole.MarkupLine("You are choosing to view the previous games.");
                if (voiceMode)
                {
                    await Task.Delay(3000);
                }
                else
                {
                    AnsiConsole.MarkupLine("Press any key to continue");
                    Console.ReadKey();
                }
                ShowPreviousGames(games,voiceMode);
                break;

            case "two":
            case "2":
                AnsiConsole.MarkupLine("You are choosing to play a new game");
                Operator operation;
                Difficulty level;

                GetNewGameConditions(out level, out operation, voiceMode);
                games.Add(PlayNewGame(operation,level, voiceMode));
                break;

            default:
                AnsiConsole.MarkupLine("Sorry, but we didn't recognize that menu option.");
                if (voiceMode)
                {
                    AnsiConsole.WriteLine("Please wait 3 seconds to try again.");
                    await Task.Delay(3000);
                }
                else
                {
                    AnsiConsole.MarkupLine("Press any key to continue");
                    Console.ReadKey();
                }
                break;
        }

    } while (userMenuChoice != "exit");

}

void ShowPreviousGames(List<Game> previousGames, bool voiceMode)
{
    Console.Clear();
    if(previousGames.Count == 0)
    {
        AnsiConsole.MarkupLine("There are no previous games to display.");
    }
    else
    {
        Grid grid = new Grid();
        grid.AddColumn();

        foreach (Game game in previousGames)
        {
            Table table = new Table().Border(TableBorder.Ascii2);
            table.AddColumn("[bold aqua]Problem:[/]");
            table.AddColumn("[bold aqua]Answer[/]");

            foreach (MathProblem problem in game.Problems)
            {
                string userAnswerColor = problem.Correct ? $"[green]{problem.UserAnswer}[/]" : $"[maroon]{problem.UserAnswer}[/]";
                table.AddRow($"[bold yellow]{problem.Num1}[/] {GetOperatorString(problem.Operation)} [bold yellow]{problem.Num2}[/] = ", userAnswerColor);
            }
            table.AddEmptyRow();
            table.AddRow("[bold aqua]Winner?[/]", (game.WinOrLose ? "[green]Winner![/]" : "[maroon]Loser.[/]"));
            table.AddRow("[bold aqua]Time to Play:[/]", game.TimeToPlay);

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }
    }
    if (voiceMode)
    {
        AnsiConsole.MarkupLine("Say 'Continue' to continue.");
        string ifContinue = "";
        while (!ifContinue.Contains('c'))
        {
            Task<String> voiceResult = GetVoiceInput();
            ifContinue = voiceResult.Result;
        }
        Thread.Sleep(2000);
    }else
    {
        AnsiConsole.MarkupLine("\nPress the Enter key to continue.");
        Console.ReadLine();
    }
}

Game PlayNewGame(Operator originalOperator, Difficulty level, bool voiceMode)
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
                userInt = GetUserAnswer(prompt, voiceMode);
                isCorrect = num1 + num2 == userInt ? true : false;
                correctAnswerCounter = num1 + num2 == userInt ? correctAnswerCounter += 1 : correctAnswerCounter += 0;
                break;

            case Operator.subtraction:
                prompt = $"{num1} - {num2} = ";
                userInt = GetUserAnswer(prompt, voiceMode);
                isCorrect = num1 - num2 == userInt ? true : false;
                correctAnswerCounter = num1 - num2 == userInt ? correctAnswerCounter += 1 : correctAnswerCounter += 0;
                break;

            case Operator.multiplication:
                prompt = $"{num1} * {num2} = ";
                userInt = GetUserAnswer(prompt, voiceMode);
                isCorrect = num1 * num2 == userInt ? true : false;
                correctAnswerCounter = num1 * num2 == userInt ? correctAnswerCounter += 1 : correctAnswerCounter += 0;
                break;

            case Operator.division:
                divisionProduct = num1 * num2;
                num2 = num1;
                num1 = divisionProduct;

                prompt = $"{num1} / {num2} = ";
                userInt = GetUserAnswer(prompt, voiceMode);
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

    AnsiConsole.MarkupLine($"\n[yellow]Dun dun dunnnnnn....... \nDid you win or lose?[/]\n\n{(didUserWin ? "[green]You won![/]" : "[maroon]You lost![/]")}");
    AnsiConsole.MarkupLine($"\nYou got {correctAnswerCounter} / 10 questions right.");
    AnsiConsole.MarkupLine($"It took you: {timeElapsed} to complete the game.");

    if (voiceMode)
    {
        AnsiConsole.MarkupLine("Say 'Continue' to continue.");
        string ifContinue = "";
        while (!ifContinue.Contains('c'))
        {
            Task<String> voiceResult = GetVoiceInput();
            ifContinue = voiceResult.Result;
        }
        Thread.Sleep(2000);
    } else
    {
        AnsiConsole.MarkupLine("\n\nPress any key to continue.");
        Console.ReadKey();
    }

    Game thisGame = new Game(currentProblems, level, didUserWin, timeElapsed);
    return thisGame;
}

void GetNewGameConditions(out Difficulty level, out Operator op, bool voiceMode)
{
    op = Operator.addition;  // operator assignment necessary to avoid Error CS0177
                             // compiler? thinks that the else statment in the outside loop is a branch that will never assign op, which is false
                             // because the while loop will repeat forever
    string? readResult;
    Task<String> voiceResult;
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

        if (voiceMode)
        {
            voiceResult = GetVoiceInput();
            userDifficultyChoice = voiceResult.Result;
        }else
        {
            readResult = Console.ReadLine();
            if (readResult != null)
            {
                userDifficultyChoice = readResult.Trim().ToLower();
            }
        }

        if (Enum.TryParse<Difficulty>(userDifficultyChoice, out level))
        {
            AnsiConsole.MarkupLine($"\nWe've recorded that you want to play a(n) [bold aqua]{level}[/] game.");

            do
            {
                AnsiConsole.MarkupLine("\nGreat! Can you also tell us which operation you'd like to play the game in?");
                AnsiConsole.MarkupLine("\nYour options are:[bold yellow] addition[/], [bold yellow]subtraction[/], [bold yellow]multiplication[/], [bold yellow]division[/], or [bold yellow]random[/].");
                AnsiConsole.MarkupLine("Random means that you may get problems in any operator.");

                if (voiceMode)
                {
                    voiceResult = GetVoiceInput();
                    userOperationChoice = voiceResult.Result;
                } else
                {
                    readResult = Console.ReadLine();
                    if (readResult != null)
                    {
                        userOperationChoice = readResult.Trim().ToLower();
                    }
                }

                if (Enum.TryParse<Operator>(userOperationChoice, out op))
                {
                    AnsiConsole.MarkupLine($"\nWe've recorded that you want to play a game using the [bold aqua]{op}[/] operation.");
                    validOperation = true;
                } else
                {
                    AnsiConsole.MarkupLine("We're sorry, but we couldn't recognize that operation type. Please try again.");
                }

                // confirm or pause execution before starting inner operation loop again
                if (voiceMode)
                {
                    AnsiConsole.WriteLine("Please wait a few seconds.");
                    Thread.Sleep(3000);
                } else
                {
                    AnsiConsole.MarkupLine("Press the Enter key to continue");
                    Console.ReadLine();
                }
            } while (!validOperation);

            validDifficulty = true;
        }else
        {
            AnsiConsole.MarkupLine("We're sorry, but we couldn't recognize that level of difficulty.");
            if (voiceMode)
            {
                AnsiConsole.WriteLine("Please wait 3 seconds to try again.");
                Thread.Sleep(3000);
            } else
            {
                AnsiConsole.MarkupLine("Press the Enter key to continue.");
                Console.ReadLine();
            }
        } 
    } while(!validDifficulty); 
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

async Task<string> GetVoiceInput()
{ 
    using SpeechRecognizer speechRecognizer = new SpeechRecognizer(speechConfig);
    SpeechRecognitionResult result = await speechRecognizer.RecognizeOnceAsync();
    int counter = 0;

    while(result.Reason != ResultReason.RecognizedSpeech)
    {
        if (counter > 0)
            // never have more than 1 'I'm sorry' line
        {
            Console.SetCursorPosition(0,Console.CursorTop-1);
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
        }
        AnsiConsole.MarkupLine("[bold maroon]I'm sorry, but I didn't recognize what you said. Please try again.[/]");

        result = await speechRecognizer.RecognizeOnceAsync();
        counter++;
    }
    AnsiConsole.MarkupLine($"[bold yellow]Voice Input Recognized:[/] {result.Text}");
    string userInput = Regex.Replace(result.Text.Trim().ToLower(), @"[^a-z0-9\s-]", "");

    return userInput;
}

int GetUserAnswer(string prompt, bool voiceMode)
{
    int userNum = -101;
    string? readResult;
    Task<String> voiceResult;
    bool validAnswer = false;

    while (!validAnswer)
    {
        AnsiConsole.Markup(prompt);

        if (voiceMode)
        {
            voiceResult = GetVoiceInput();
            readResult = voiceResult.Result;
        } else
        {
            readResult = Console.ReadLine();
        }

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

