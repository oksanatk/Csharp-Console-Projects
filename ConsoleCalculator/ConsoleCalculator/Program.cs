using Spectre.Console;
using CalculatorLibrary;
using Microsoft.CognitiveServices.Speech;
using System.Text.RegularExpressions;

bool SpeechRecognitionInput = false;
string? speechKey = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Key");
string? speechRegion = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Region");
SpeechConfig speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
speechConfig.SpeechRecognitionLanguage = "en-US";  

string? readResult;
string[] validOperations = new string[] { "add", "sub", "mult", "div", "sq", "pow", "10x", "sin", "cos", "tan" };
string[] singleNumberOperations = new string[] { "sq", "10x", "sin", "cos", "tan" }; 
Calculator calculator = new Calculator();
bool endApp = false;


if (args.Contains("--voice-input"))
{
    SpeechRecognitionInput = true;
}

await ShowMainMenu(SpeechRecognitionInput);

async Task ShowMainMenu(bool voiceMode)
{
    while (!endApp)
    {
        double num1 = double.NaN;
        double num2 = double.NaN;
        string op = "";
        double result = double.NaN;

        do
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold yellow]Console Calculator in C#[/]");
            AnsiConsole.MarkupLine("[bold yellow]------------------------\n[/]");

            Panel operationsPanel = ShowOperationsPanel();
            AnsiConsole.Write(operationsPanel);
            
            AnsiConsole.MarkupLine("[bold yellow]OR[/] \nEnter [aqua]num[/] to see the number of times the calculator has been used.");
            AnsiConsole.MarkupLine("Enter [aqua]calc[/] to see the previous calculations");
            AnsiConsole.MarkupLine("Enter [aqua]exit[/] to exit the app.");
            AnsiConsole.Markup("\n[bold yellow]Your option? [/]");

            if (voiceMode)
            {
                op = await GetVoiceInput();
            } else
            {
                readResult = Console.ReadLine();
                if (readResult != null)
                {
                    op = readResult.Trim().ToLower();
                } 
            }

            if (op == "exit") { endApp = true; }

            else if (op.StartsWith("num")) 
            { 
                AnsiConsole.MarkupLine("\nThe calculator has been used [bold yellow]{0}[/] times.", calculator.TimesUsed);
                if (voiceMode)
                {
                    AnsiConsole.MarkupLine("\nPlease wait a few seconds to continue back to the main menu.");
                    Thread.Sleep(3000);
                    //TODO test above
                } else
                {
                    AnsiConsole.MarkupLine("\nPress the [yellow]Enter[/] key to continue back to the main menu.");
                    Console.ReadLine();
                }

            }
            else if (op == "calc")
            {
                Console.Clear();
                AnsiConsole.MarkupLine("You are choosing to view previous operations.");
                AnsiConsole.Write(calculator.ShowPreviousCalculations(calculator.RecentCalculations));

                if (voiceMode)
                {
                    AnsiConsole.MarkupLine("\nSay [aqua]clear[/] to clear the history of recent calculations, or say [yellow]Continue[/] to continue.");
                    readResult = GetVoiceInput().Result;
                    //TODO test above
                } else
                {
                    AnsiConsole.MarkupLine("\nEnter [aqua]clear[/] to clear the history of recent calculations, or just press the [yellow]Enter[/] key to continue.");

                    readResult = Console.ReadLine();
                    if (readResult != null)
                    {
                        readResult = readResult.Trim().ToLower();
                    }
                }

                if (readResult != null && readResult.StartsWith("c"))
                {
                    calculator.ClearCalculations();
                } 
            }
            else if (validOperations.Contains(op))
            {
                AnsiConsole.MarkupLine("You're choosing the [bold yellow]{0}[/] operation.", op);
            }
            else
            {
                if (voiceMode)
                {
                    AnsiConsole.MarkupLine("[maroon]I'm sorry, but I didn't understand that operator. Press try again.[/]");
                } else
                {
                    AnsiConsole.MarkupLine("[maroon]I'm sorry, but I didn't understand that operator. Press [yellow]Enter[/] to try again.[/]");
                    Console.ReadLine();
                }

            }
        } while (!validOperations.Contains(op) && op !="exit");

        if (op != "exit" && op !=null)
        {
            num1 = GetNum(false, voiceMode);

            // if not single-num operation, get num2
            if (!singleNumberOperations.Contains(op))
            {
                num2 = GetNum(true, voice);
            }

            result = calculator.DoOperation(num1, num2, op);
            if (double.IsNaN(result))
            {
                AnsiConsole.MarkupLine("[bold maroon]This operation results in a mathematical error.[/]");
            }
            else AnsiConsole.MarkupLine("Your result is: [bold yellow]{0}[/]\n", result);

            AnsiConsole.MarkupLine("\t------------------------\n");
            AnsiConsole.Markup("Enter [yellow]exit[/] to close the app, or press the 'Enter' key to continue. ");

            if (voiceMode)
            {
                if (GetVoiceInput().Result == "exit") endApp = true;
            } else
            {
                if (Console.ReadLine() == "exit") endApp = true;
            }              
            AnsiConsole.MarkupLine("\n");      
        }
    }
    calculator.Finish();
}

double GetNum(bool gettingSecondNum, bool voiceMode)
{
    double userNum = double.NaN;
    bool validNum = false;
    do
    {
        if (gettingSecondNum)
        {
            AnsiConsole.MarkupLine("\nPlease enter a second number.");
        }else
        {
            AnsiConsole.MarkupLine("\nPlease enter a number.");

        }
        AnsiConsole.MarkupLine("OR enter [aqua]calc[/] to view the previous calculations and results.");

        if (voiceMode)
        {
            readResult = GetVoiceInput().Result;
        } else
        {
            readResult = Console.ReadLine();
        }

        if (readResult != null)
        {
            if (readResult.StartsWith("calc"))
            {
                AnsiConsole.Write(calculator.ShowPreviousCalculations(calculator.RecentCalculations));
                AnsiConsole.MarkupLine("[yellow]\n\t---------------------------\n[/]");

                AnsiConsole.MarkupLine("Type [yellow]calculation #[/] to use the result of a previous calculation. IE, [yellow]calculation 1[/]");
                AnsiConsole.MarkupLine("OR enter any number (##) to use it instead.");

                if (voiceMode)
                {
                    readResult = GetVoiceInput().Result;
                } else
                {
                    readResult = Console.ReadLine();
                }

                if (readResult != null && readResult.StartsWith("calc"))
                {
                    string[] parseCalculationNumber = readResult.Split(' ');
                    int previousCalculation = 0;
                    if (int.TryParse(parseCalculationNumber[1],out previousCalculation))
                    {
                        if ((previousCalculation > 0) && (previousCalculation <= calculator.RecentCalculations.Count))
                        {
                            userNum = calculator.RecentCalculations[(previousCalculation - 1)].Result;
                            AnsiConsole.MarkupLine($"Recording your number as [bold yellow]{userNum}[/]");
                            return userNum;

                        } else
                        {
                            AnsiConsole.MarkupLine("[maroon]Sorry, but we couldn't find a corresponding calculation result.[/]");
                        }
                    } else
                    {
                        AnsiConsole.MarkupLine("[maroon]Sorry, but I couldn't understand that number. Press the [yellow]Enter[/] key to try again.[/]");
                    }
                }
            }
            
            if (double.TryParse(readResult, out userNum))
            {
                validNum = true;
            }
            else if (readResult != null && !readResult.StartsWith("calc"))
            {
                if (voiceMode)
                {
                    AnsiConsole.MarkupLine("[maroon]Sorry, but I couldn't understand that number. Please try again.[/]");
                } else
                {
                    AnsiConsole.MarkupLine("[maroon]Sorry, but I couldn't understand that number. Press the [yellow]Enter[/] key to try again.[/]");
                    Console.ReadLine(); 
                }
            }
        }
    } while (!validNum);
    return userNum;
}

Panel ShowOperationsPanel()
{
    Grid grid = new Grid();
    grid.AddColumn();

    grid.AddRow("[aqua]add[/]  - Add");
    grid.AddRow("[aqua]sub[/]  - Subtract");
    grid.AddRow("[aqua]mult[/] - Multiply");
    grid.AddRow("[aqua]div[/]  - Divide");
    grid.AddRow("[aqua]sq[/]   - Square Root of a Number");
    grid.AddRow("[aqua]pow[/]  - Raise to the Power");
    grid.AddRow("[aqua]10x[/]  - 10");
    grid.AddRow("[aqua]sin[/]  - Sine of a Number");
    grid.AddRow("[aqua]cos[/]  - Cosine of a Number");
    grid.AddRow("[aqua]tan[/]  - Tangent of a Number");

    return new Panel(grid)
    {
        Header = new PanelHeader("[yellow]Choose an Operation:[/]"),
        Border = BoxBorder.Rounded,
        Padding = new Padding(1)
    };
}

async Task<String> GetVoiceInput()
{
    string userVoiceInput = "";
    using SpeechRecognizer recognizer = new SpeechRecognizer(speechConfig);
    RecognitionResult recognizedInput;

    do
    {
        recognizedInput = await recognizer.RecognizeOnceAsync();

        if (recognizedInput.Reason == ResultReason.RecognizedSpeech)
        {
            userVoiceInput = recognizedInput.Text;
            AnsiConsole.MarkupLine($"[bold yellow]Recognizer Voice Input[/]: {userVoiceInput}");

            userVoiceInput = userVoiceInput.Trim().ToLower();
            userVoiceInput = Regex.Replace(userVoiceInput, @"[^a-z0-9\s-]", "");

            return userVoiceInput;
        } 
        else if (recognizedInput.Reason == ResultReason.Canceled)
        {
            AnsiConsole.MarkupLine($"[maroon]SPEECH RECOGNITION CANCELLED:[/] {CancellationDetails.FromResult(recognizedInput)}\n");
            endApp = true;
            return "exit";
        }
        else
        {
            AnsiConsole.MarkupLine(recognizedInput.Reason.ToString());
            AnsiConsole.MarkupLine("[maroon]I'm sorry, I didn't understand that input.[/]");
        } 
    } while (recognizedInput.Reason != ResultReason.RecognizedSpeech);
    return userVoiceInput;
}


