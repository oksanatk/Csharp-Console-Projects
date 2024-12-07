using Microsoft.Data.Sqlite;
using Microsoft.CognitiveServices.Speech;
using System.Text.RegularExpressions;

string? readResult;
bool userSpeechInput = false;
string? speechKey = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Key");
string? speechRegion = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Region");

if (!File.Exists("Habits.db")) { File.Create("Habits.db"); }

if (args.Contains("--voice-input")) { userSpeechInput = true; }


DeleteExistingHabit(ReadHabitsFromFile(), userSpeechInput);
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
                DeleteExistingHabit(currentHabits,voiceMode);
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

                switch (userEditSelection)
                {
                    case "update":
                        UpdateRecord(userHabitInput, voiceMode);
                        //TODO - method not made yet
                        break;

                    case "add":
                        Console.WriteLine($"You're choosing to add a new record.");
                        AddNewRecordToHabit(userHabitInput, voiceMode);
                        break;

                    case "delete":
                        DeleteRecord(userHabitInput, voiceMode);
                        // TODO - method not made yet
                        break;

                    default:
                        Console.WriteLine("I'm sorry, but I didn't understand that. Please try again.");
                        break;
                }

            }
        }

    }
}

void DeleteExistingHabit(List<String> currentHabits, bool voiceMode)
{
    bool validExistingHabit = false;
    string userSelectedHabit = "";
    string? userConfirmation ="";
    string? readResult;

    while (!validExistingHabit)
    {
        Console.Clear();
        Console.WriteLine("Here's a list of the current habits you are tracking.");
        currentHabits.ForEach(x => Console.WriteLine(x));
        Console.WriteLine("\t--------------------\n");
        
        Console.WriteLine("Please enter the habit you would like to delete. Or enter 'exit' to exit back to the main screen.");

        if (voiceMode)
        {
            userSelectedHabit = GetVoiceInput().Result;
        } else
        {
            readResult = Console.ReadLine();
            if (readResult != null)
            {
                userSelectedHabit = readResult.Trim().ToLower();
                readResult = null;
            }
        }

        if (userSelectedHabit != null && currentHabits.Contains(userSelectedHabit))
        {
            Console.WriteLine($"You are choosing to delete the habit {userSelectedHabit}. Please confirm if you would like to continue (y/n).");
            if (voiceMode)
            {
                userConfirmation = GetVoiceInput().Result;
            } else
            {
                readResult = Console.ReadLine();
                if (readResult != null)
                {
                    userConfirmation = readResult.Trim().ToLower();
                    readResult = null;
                }
            }
            if (userConfirmation != null && userConfirmation.StartsWith("y"))
            {
                validExistingHabit = true;
            }
        }
        if (userSelectedHabit == "exit") { break; }
    }

    if (userSelectedHabit!=null && validExistingHabit)
    {
        using (SqliteConnection connection = new SqliteConnection("DataSource=Habits.db"))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
                $@"
                    DROP TABLE IF EXISTS {userSelectedHabit};
                ";

            command.ExecuteNonQuery();
        }

        bool validContinue = false;
        string continueConfirmation = "";

        Console.WriteLine($"Your habit {userSelectedHabit} has been successfully deleted.");
        do
        {
            if (voiceMode)
            {            
                Console.WriteLine("Say 'continue' to continue back to the main screen.");
                continueConfirmation = GetVoiceInput().Result;
                if (continueConfirmation.StartsWith("c"))
                {
                    validContinue = true;
                }
            } else
            {
                Console.WriteLine("Press the 'Enter' key to continue back to the main menu.");
                Console.ReadLine();
                validContinue = true;
            }
        } while (!validContinue);
        
    }
}

void DeleteRecord(string habit, bool voiceMode) { }

void UpdateRecord(string habit, bool voiceMode) { }

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
    List<string> datePieces = new();
    int userQuantity = -1;

    habit = Regex.Replace(habit, @"[\s]", "_");
    habit = Regex.Replace(habit, @"[^a-zA-Z0-9_]", "");

    Console.WriteLine("We need the date for the new record of your habit. Please enter the year of the date (yyyy)");
    datePieces.Add(GetUserIntInput(voiceMode).ToString());
  
    Console.WriteLine("Please enter the DAY of the month for the date of your new habit record (dd)");
    datePieces.Add(GetUserIntInput(voiceMode).ToString());

    Console.WriteLine("Please enter the month of the date of your new habit record (mm)");
    datePieces.Add(GetUserIntInput(voiceMode).ToString());

    userDateInputted = String.Join("-", datePieces);

    Console.WriteLine("Please enter the quantity to record");
    userQuantity = GetUserIntInput(voiceMode);

    using (SqliteConnection connection = new SqliteConnection("DataSource=Habits.db"))
    {
        connection.Open();

        SqliteCommand validateUnitOfMeasureName = connection.CreateCommand();
        validateUnitOfMeasureName.CommandText = $"PRAGMA table_info({habit});";

        string? unitOfMeasureName = "";

        using (SqliteDataReader reader = validateUnitOfMeasureName.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine("fieldcount: " + reader.FieldCount);
                if (reader[0].ToString()=="1" && reader[1] != null)
                {
                    unitOfMeasureName = reader[1].ToString();
                }
            }
        }
        
        var command = connection.CreateCommand();
        command.CommandText =
            $@"
                INSERT INTO {habit}
                ({unitOfMeasureName}, date_only) VALUES
                (@userQuantity, @userDateInputted);
            ";
        command.Parameters.AddWithValue("@userQuantity", userQuantity);
        command.Parameters.AddWithValue("@userDateInputted", userDateInputted);

        command.ExecuteNonQuery();
    }
}

int GetUserIntInput(bool voiceMode)
{
    string? maybeNumber = "";
    bool validNumber = false;
    int realNumber = -1;

    do
    {
        if (voiceMode)
        {
            maybeNumber = GetVoiceInput().Result;
        } else
        {
            maybeNumber = Console.ReadLine();
        }
        
        if (int.TryParse(maybeNumber,out realNumber))
        {
            validNumber = true;
        } else
        {
            Console.WriteLine("I'm sorry, but I didn't understand that number. Please try again.");
        }
    } while (!validNumber);

    return realNumber;
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
