using Microsoft.Data.Sqlite;
using Microsoft.CognitiveServices.Speech;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

string? readResult;
bool userSpeechInput = false;
string? speechKey = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Key");
string? speechRegion = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Region");

if (!File.Exists("Habits.db")) { File.Create("Habits.db"); }

if (args.Contains("--voice-input")) { userSpeechInput = true; }

//ShowMainMenu(userSpeechInput);
ReadHabitsFromFile();

void ShowMainMenu(bool voiceMode)
{
    bool endApp = false;
    string userMenuOption = "";

    while (!endApp)
    {
        List<String> currentHabits = ReadHabitsFromFile();

        Console.Clear();
        Console.WriteLine("Welcome to your personal Habit Logger.\n");
        Console.WriteLine("Please select what you'd like to do:");
        Console.WriteLine("\t1. Create - Create a new habit");
        Console.WriteLine("\t2. Edit - Edit or Update an existing habit");
        Console.WriteLine("\t3. Delete - delete all data relating to a habit.");
        Console.WriteLine("\t4. Report - view statistics about all habits.");
        Console.WriteLine("\nOR enter 'exit' to exit the application.");


        if (voiceMode)
        {
            userMenuOption = GetVoiceInput().Result;
        }
        else
        {
            readResult = Console.ReadLine();
            if (readResult != null)
            {
                userMenuOption = readResult.Trim().ToLower();
            }
        }

        switch (userMenuOption)
        {
            case "create":
                CreateNewHabit(currentHabits, voiceMode);
                // do i need a function to read the existing habits from the database and turn it into a string?
                break;
            case "edit":
                EditExistingHabit();
                break;
            case "delete":
                DeleteAHabit();
                break;
            case "report":
                ViewHabitReport();
                break;
            case "exit":
                endApp = true;
                break;

            default:
                if (voiceMode)
                {
                    Console.WriteLine("I'm sorry, but I didn't understand that menu option. Please try again in a few seconds.");
                    Thread.Sleep(2500);
                } else
                {
                    Console.WriteLine("I'm sorry, but I didn't understand that menu option. Press 'Enter' to try again.");
                    Console.ReadLine();
                }
                break;
        }

    }

}

void CreateNewHabit(List<String> currentHabits, bool voiceMode)
{
    string userHabitInput = "";
    string userUnitOfMeasure = "";
    bool validHabitSelected = false;

    Console.WriteLine("\n\t Here are the current habits you are logging: ");
    currentHabits.ForEach(h => Console.WriteLine(h));
    while (!validHabitSelected)
    {
        if (voiceMode)
        {
            Console.WriteLine("\nPlease say the name of a new habit you would like to track.");
            userHabitInput = GetVoiceInput().Result;
        }
        else
        {
            string? readResult = Console.ReadLine();
            if (readResult != null)
            {
                userHabitInput = readResult.Trim().ToLower();
            }
        }
        if (!currentHabits.Contains(userHabitInput))
        {
            validHabitSelected = true;
            Console.WriteLine($"You are choosing to create a new habit called {userHabitInput}.");
            Console.WriteLine("What would you like to choose to be the unit of measure?");

            if (voiceMode)
            {
                Console.WriteLine($"\nPlease say the unit of measure you would like to use to track your {userHabitInput} habit.");
                userUnitOfMeasure = GetVoiceInput().Result;
            }
            else
            {
                string? readResult = Console.ReadLine();
                if (readResult != null)
                {
                    userUnitOfMeasure = readResult.Trim().ToLower();
                }
            }
            Console.WriteLine($"You are choosing {userUnitOfMeasure} to be the unit of measure.");
        }
        else
        {
            if (voiceMode)
            {
                Console.WriteLine("I'm sorry, but you already have a habit with that name. Please try again.");
            }
            else
            {
                Console.WriteLine("I'm sorry, but you already have a habit with that name. Please press 'Enter' to try again.");
                Console.ReadLine();
            }
        }
    }

    using (SqliteConnection connection = new SqliteConnection("DataSource=Habits.db"))
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            @"
                CREATE TABLE $userHabitInput (
                 id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                 $userUnitOfMeasure INTEGER NOT NULL,
                 date_only TEXT NOT NULL
                );  
            ";
        command.Parameters.AddWithValue("$userHabitInput",userHabitInput);
        command.Parameters.AddWithValue("$userUnitOfMeasure", userUnitOfMeasure);
        command.ExecuteNonQuery();
    }

    Console.WriteLine($"New habit called {userHabitInput} with the unit of measure {userUnitOfMeasure} has been created!");
}

void EditExistingHabit() { }

void DeleteAHabit() { }

void ViewHabitReport() { }

List<String> ReadHabitsFromFile()
{
    List<String> currentHabits = new();

    using (var connection = new SqliteConnection("DataSource=Habits.db"))
    {
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = 
            @" 
                SELECT * 
                FROM sqlite_master
                WHERE type='table'
            ";

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                currentHabits.Add(reader.GetString(1));
            }
        }
    }
    return currentHabits;
}



async Task<String> GetVoiceInput()
{
    int repeatCounter = 0;
    RecognitionResult result;
    SpeechConfig speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
    speechConfig.SpeechRecognitionLanguage = "en-US";

    using SpeechRecognizer recognizer = new SpeechRecognizer(speechConfig);

    do
    {
        result =  await recognizer.RecognizeOnceAsync();

        if (result.Reason == ResultReason.RecognizedSpeech)
        {
            string userVoiceInput = result.Text;
            Console.WriteLine("  Speech Input Recognized: {userVoiceInput}");

            userVoiceInput = userVoiceInput.Trim().ToLower();
            Regex.Replace(userVoiceInput, @"[^a-z0-9\s]", "");

            return userVoiceInput;

        } else if (result.Reason == ResultReason.Canceled)
        {
            Console.WriteLine($"An error occured during speech recognition: \n\t{CancellationDetails.FromResult(result)}");
        } else
        {
            if (repeatCounter <1)
            {
                Console.WriteLine("I'm sorry, but I didn't understand what you said. Please try again.");
            }
            repeatCounter++;
        }
    } while (result.Reason != ResultReason.RecognizedSpeech);

    return "UnexpectedVoiceResult Error";
}

void HelloWorldSQLite()
{
    using (var connection = new SqliteConnection("Data Source=testCode.db"))
    {
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            @"
            CREATE TABLE user (
                id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL
            );
    
            INSERT INTO user
            VALUES (1,'Brice'),
                   (2,'Alexander'),
                   (3,'Nate');
        ";

        command.ExecuteNonQuery();

        Console.Write("Name: ");
        var name = Console.ReadLine();

        command.CommandText =
            @"
            INSERT INTO user (name)
            VALUES ($name)
        ";
        command.Parameters.AddWithValue("$name", name);
        command.ExecuteNonQuery();

        command.CommandText =
            @"
            SELECT last_insert_rowid()
        ";
        var newId = (long)command.ExecuteScalar();

        Console.WriteLine($"Your new user ID is {newId}");
    }

    Console.Write("User ID: ");
    var id = int.Parse(Console.ReadLine());

    using (var connection = new SqliteConnection("Data Source=testCode.db"))
    {
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText =
            @"
            SELECT name
            FROM user
            WHERE id = $id
        ";
        command.Parameters.AddWithValue("$id", id);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var name = reader.GetString(0);
                Console.WriteLine($"Hello, {name}");
            }
        }

    }

    SqliteConnection.ClearAllPools();
    File.Delete("testCode.db");
}
