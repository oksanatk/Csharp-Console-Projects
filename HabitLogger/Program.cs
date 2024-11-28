using Microsoft.Data.Sqlite;
using Microsoft.CognitiveServices.Speech;
using System.Text.RegularExpressions;

string? readResult;
bool userSpeechInput = false;
string? speechKey = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Key");
string? speechRegion = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Region");

if (!File.Exists("Habits.db")) { File.Create("Habits.db"); }

if (args.Contains("--voice-input")) { userSpeechInput = true; }


ReadRecordsFromHabit("sample_habit1");
ShowMainMenu(userSpeechInput);

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
            case "1":
                CreateNewHabit(currentHabits, voiceMode);
                // do i need a function to read the existing habits from the database and turn it into a string?
                break;
            case "edit":
            case "2":
                EditExistingHabit(currentHabits, voiceMode);
                break;
            case "delete":
            case "3":
                DeleteAHabit();
                break;
            case "report":
            case "4":
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

    Console.WriteLine("\nHere are the current habits you are logging: \n");
    currentHabits.ForEach(h => Console.WriteLine(h));
    Console.WriteLine("\t--------------------\n");
    while (!validHabitSelected)
    {
        if (voiceMode)
        {
            Console.WriteLine("\nPlease say the name of a new habit you would like to track.");
            userHabitInput = GetVoiceInput().Result;
        }
        else
        {
            Console.WriteLine("\nPlease type the name of a new habit you would like to track.");
            string? readResult = Console.ReadLine();
            if (readResult != null)
            {
                userHabitInput = readResult.Trim().ToLower();
            }
        }
        if (!currentHabits.Contains(userHabitInput))
        {
            validHabitSelected = true;
            Console.WriteLine($"\nYou are choosing to create a new habit called {userHabitInput}.");
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

        userHabitInput = Regex.Replace(userHabitInput, @"[\s]","_");
        userHabitInput = Regex.Replace(userHabitInput, @"[^a-zA-Z0-9_]", "");

        userUnitOfMeasure = Regex.Replace(userUnitOfMeasure, @"[\s]", "_");
        userUnitOfMeasure = Regex.Replace(userUnitOfMeasure, @"[^a-zA-Z0-9_]", "");

        command.CommandText =
            $@"
                CREATE TABLE IF NOT EXISTS {userHabitInput} (
                 id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                 {userUnitOfMeasure} INTEGER NOT NULL,
                 date_only TEXT NOT NULL
                );  
            "; // cannot parameterize table or column names

        command.ExecuteNonQuery();
    }

    Console.WriteLine($"New habit called {userHabitInput} with the unit of measure {userUnitOfMeasure} has been created!");
}

void EditExistingHabit(List<String> currentHabits, bool voiceMode)
{
    string userHabitInput = "";
    string userEditSelection = "";
    bool validHabitSelected = false;
    bool validAddOrEditOption = false;

    Console.WriteLine("\n\t Here are the current habits you are logging: ");
    currentHabits.ForEach(h => Console.WriteLine(h));
    Console.WriteLine("\t--------------------\n");

    // select which habit to modify, and whether user would prefer to add a new entry or update an existing one. 

    while (!validHabitSelected)
    {
        Console.WriteLine("Please select which of the above habits you'd like to modify.");
        if (voiceMode)
        {
            userHabitInput = GetVoiceInput().Result;
        }
        else
        {
            string? readResult = Console.ReadLine();
            if (readResult != null)
            {
                userHabitInput = readResult;
            }
        }
        if (currentHabits.Contains(userHabitInput))
        {
            validHabitSelected = true;
            // TODO view all records of that habit?
            ReadRecordsFromHabit(userHabitInput);
               
            while (!validAddOrEditOption)
            {
                Console.WriteLine($"Great! Please select whether you'd like to 'update' or 'add' or 'delete' a record.");
                if (voiceMode)
                {
                    userEditSelection = GetVoiceInput().Result;
                }
                else
                {
                    string? readResult = Console.ReadLine();
                    if (readResult != null)
                    {
                        userEditSelection = readResult.Trim().ToLower();
                    }
                }
                // TODO -- switch on userEditSelection

                if (userEditSelection == "update")
                {
                    /// METHOD HERE TODO
                } else if (userEditSelection == "add")
                {
                    /// TODO METHOD HERE
                    Console.WriteLine($"You're choosing to add a new record.");
                    AddNewRecordToHabit(userHabitInput,voiceMode);

                } else
                {
                    Console.WriteLine("I'm sorry, but I didn't understand that. Please try again.");
                }
            }
        }

    }
}

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

void ReadRecordsFromHabit(string habit)
{
    Regex.Replace(habit, @"[^a-zA-Z0-9_]", ""); // remove non-alphanumeric to reduce risk of sql injection
    string commandText = $"SELECT * FROM {habit};"; // cannot parameterize table names, must format command string manually

    using (var connection = new SqliteConnection("DataSource=Habits.db"))
    {
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = commandText;

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($"{reader.GetName(i)}\t");
            }
            Console.WriteLine();
            while (reader.Read())
            {         
                // TODO - don't know where the need to store is coming from - i can look up the query by id later 
                // since it should be created as an autoincrement when the 
                Console.WriteLine($"{reader.GetString(0)}\t{reader.GetString(1)}\t{reader.GetString(2)}");
            }
        }
    }

}

void AddNewRecordToHabit(string habit,bool voiceMode)
{
    string userDateInputted = "";
    int userQuantity;
    string? readResult;
    bool validNumEntered = false;

    habit = Regex.Replace(habit, @"[\s]", "_");
    habit = Regex.Replace(habit, @"[^a-zA-Z0-9_]", "");

    Console.WriteLine("Please enter the date of the new record (yyyy-dd-mm");
    if (voiceMode)
    {
        //TODO need to modify voice input validation so that date can be said in a more user-friendly way
        
    } else
    {
        userDateInputted = GetUserDateInput();
    }

    while (!validNumEntered)
    {
        Console.WriteLine("Please enter the quantity to record");
        if (voiceMode)
        {
            readResult = GetVoiceInput().Result;
        }
        else
        {
            readResult = Console.ReadLine();
        } 
      if (int.TryParse(readResult, out userQuantity))
        {
            validNumEntered = true;
        }
    }
    // TODO sqlite connection and executeNonQuery() to the 
}

string GetUserDateInput()
{
    return "";
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
