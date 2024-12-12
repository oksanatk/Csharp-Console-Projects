using Microsoft.Data.Sqlite;
using Microsoft.CognitiveServices.Speech;
using System.Text.RegularExpressions;
using Microsoft.CognitiveServices.Speech.Speaker;

string? readResult;
bool userSpeechInput = false;
string? speechKey = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Key");
string? speechRegion = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Region");

if (!File.Exists("Habits.db")) 
{ 
    File.Create("Habits.db").Close(); 
    AutoPopulateSampleData();
}

if (args.Contains("--voice-input")) { userSpeechInput = true; }


ShowMainMenu(userSpeechInput);

void ShowMainMenu(bool voiceMode)
{
    bool endApp = false;
    string userMenuOption = "";

    while (!endApp)
    {
        List<String> currentHabits = ReadHabitsFromFile();
        currentHabits.Remove("sqlite_sequence");

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
                ViewHabitReport(currentHabits, voiceMode); // TODO -- method not complete, need to create still
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

    Console.WriteLine("\n\tHere are the current habits you are logging: \n");
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
                readResult = null;
            }
        }
        if (currentHabits.Contains(userHabitInput))
        {
            validHabitSelected = true;
               
            while (!validAddOrEditOption)
            {
                Console.WriteLine($"\nGreat! Please select whether you'd like to 'update' or 'add' or 'delete' a record. Or enter 'exit' to return to the main menu.");
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
                        readResult = null;
                    }
                }

                switch (userEditSelection)
                {
                    case "update":
                        UpdateRecord(userHabitInput, voiceMode);
                        validAddOrEditOption = true;
                        break;

                    case "add":
                        Console.WriteLine($"You're choosing to add a new record.");
                        AddNewRecordToHabit(userHabitInput, voiceMode);
                        validAddOrEditOption = true;
                        break;

                    case "delete":
                        DeleteRecord(userHabitInput, voiceMode);
                        validAddOrEditOption = true;
                        break;

                    case "exit":
                        validAddOrEditOption= true;
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
        // TOOD -- feature idea: 
        //      maybe use regex so that user can type spaces instead of underscores, and automatically replace them for the sql query? 
        //          that'd be cool

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

void DeleteRecord(string habit, bool voiceMode)
{
    int userSelectedId;
    habit = habit.Trim().ToLower();
    habit = Regex.Replace(habit, @"[^a-zA-Z0-9_]", ""); 

    ReadRecordsFromHabit(habit);
    Console.WriteLine("Please select the id (#) of the habit you would like to delete.");
    userSelectedId = GetUserIntInput(voiceMode); 

    using (SqliteConnection connection = new SqliteConnection("DataSource=Habits.db"))
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            $@"
                DELETE FROM {habit} WHERE id=@userSelectedId;
            ";

        command.Parameters.AddWithValue("@userSelectedId", userSelectedId);
        command.ExecuteNonQuery();
    }
    if (voiceMode)
    {
        Console.WriteLine($"Your record for the habit {habit}, and the id {userSelectedId} is no more. Please wait a few seconds to continue.");
        Thread.Sleep(3000);
    } else
    {
        Console.WriteLine($"Your record for the habit {habit}, and the id {userSelectedId} is no more. Press the 'Enter' key to continue.");
        Console.ReadLine();
    }
}

void UpdateRecord(string habit, bool voiceMode)
{
    int userSelectedId = -1;
    int userSelectedQuantity = -1;
    string userSelectedDate = "";
    string habitUnitOfMeasure = "";

    habit = habit.Trim().ToLower();
    habit = Regex.Replace(habit, @"[^a-zA-Z0-9_]", "");

    ReadRecordsFromHabit(habit);
    Console.WriteLine("Please select the id (#) of the record you would like to update.");
    userSelectedId = GetUserIntInput(voiceMode);

    Console.WriteLine($"Now we'll need to get the new date you would like to update entry with id #{userSelectedId} to.");
    userSelectedDate = GetUserDateInput(voiceMode);

    Console.WriteLine($"Please enter the new quantity (##) you would like to record.");
    userSelectedQuantity = GetUserIntInput(voiceMode);

    using (SqliteConnection connection = new SqliteConnection("DataSource=Habits.db"))
    {
        connection.Open();

        habitUnitOfMeasure = ReadUnitOfMeasureFromHabit(connection, habit);
        habitUnitOfMeasure = habitUnitOfMeasure.Trim().ToLower();
        habitUnitOfMeasure = Regex.Replace(habitUnitOfMeasure, @"[^a-zA-Z0-9_]", "");

        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            $@"
                INSERT INTO {habit} (id, {habitUnitOfMeasure}, date_only)
                VALUES (@userSelectedId, @userSelectedQuantity, '@userSelectedDate)
                    ON CONFLICT(id) DO UPDATE SET
                    {habitUnitOfMeasure} = excluded.{habitUnitOfMeasure},
                    date_only = excluded.date_only;
            ";
        command.Parameters.AddWithValue("@userSelectedId", userSelectedId);
        command.Parameters.AddWithValue("@userSelectedQuantity", userSelectedQuantity);
        command.Parameters.AddWithValue("@userSelectedDate", userSelectedDate);

        command.ExecuteNonQuery();
    }
    Console.WriteLine($"The entry in the habit {habit} and id #{userSelectedId} has been updated to {userSelectedQuantity} {habitUnitOfMeasure} on {userSelectedDate}. ");
}

void AddNewRecordToHabit(string habit,bool voiceMode)
{
    bool exitToMainMenu = false;
    string userDateInputted = "";
    int userQuantity = -1;

    habit = Regex.Replace(habit, @"[\s]", "_");
    habit = Regex.Replace(habit, @"[^a-zA-Z0-9_]", "");

    ReadRecordsFromHabit(habit);

    Console.WriteLine("To add a new entry, we'll first need the date of the new entry.\n");
    userDateInputted = GetUserDateInput(voiceMode);

    Console.WriteLine("Please enter the quantity to record");
    userQuantity = GetUserIntInput(voiceMode);
    
    // confirm or exit to main menu
    string confirmationMessage = "";
    Console.WriteLine($"You're choosing to add a new record to the habit {habit} with the quantity {userQuantity} and date {userDateInputted} (yyyy-dd-mm). Please confirm (y/n).");
    if (voiceMode)
    {
        confirmationMessage = GetVoiceInput().Result;
    } else
    {
        readResult = Console.ReadLine();
        if (readResult != null)
        {
            confirmationMessage = readResult.Trim().ToLower();
        }
    }
    if (!confirmationMessage.Contains("y"))
    {
        exitToMainMenu = true;
    }

    if (!exitToMainMenu)
    {
        using (SqliteConnection connection = new SqliteConnection("DataSource=Habits.db"))
        {
            connection.Open();
            string unitOfMeasureName = ReadUnitOfMeasureFromHabit(connection, habit);

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
}

void ViewHabitReport(List<String> currentHabits, bool voiceMode)
{
    bool backToMainMenu = false;
    bool validHabitSelected = false;
    string userContinue = "";
    string? readResult;
    string userHabitSelection = "";
    string habitUnitOfMeasure = "";

    List<List<String>> specialtyStatQueries = new List<List<String>>()
    {
        new List<String> {"All-Time Number of Records: ", @" SELECT COUNT(*) FROM @userHabitSelection;" },

        new List<String> {"Number of Records in Past Year (Past 365 days): ", @" SELECT COUNT(*) FROM @userHabitSelection WHERE date_only >= date('now', '-365 days');" },

        new List<String> {"Total @habitUnitOfMeasure Recorded All-Time: ", @" SELECT SUM(@habitUnitOfMeasure) FROM @userHabitSelection;" },

        new List<String> {"Total @habitUnitOfMeasure for Past Year: ", @" SELECT SUM(@habitUnitOfMeasure) FROM @userHabitSelection WHERE date_only >= date('now', '-365 days');" },

        new List<String> {"Average @habitUnitOfMeasure All-Time: ", @" SELECT AVG(@habitUnitOfMeasure) FROM @userHabitSelection;" },

        new List<String> {"Average @habitUnitOfMeasure for Past Year: ", @" SELECT AVG(@habitUnitOfMeasure) FROM @userHabitSelection WHERE date_only >= date('now', '-365 days');" },

        new List<String> {"Highest Number of @habitUnitOfMeasure Ever: ", @" SELECT MAX(@habitUnitOfMeasure) FROM @userHabitSelection;" },

        new List<String> {"Lowest Number of @habitUnitOfMeasure Ever: ", @" SELECT MIN(@habitUnitOfMeasure) FROM @userHabitSelection;" }
    };

    Console.Clear();
    Console.WriteLine("\n\tHere are the current habits you can view a report for: \n");
    currentHabits.ForEach(h => Console.WriteLine(h));
    Console.WriteLine("\t--------------------\n");

    while (!validHabitSelected)
    {
        Console.WriteLine("Please select the habit you would like to view statistics for.");
        if (voiceMode)
        {
            userHabitSelection = GetVoiceInput().Result;
        }
        else
        {
            readResult = Console.ReadLine();
            if (readResult != null)
            {
                userHabitSelection = readResult.Trim().ToLower();
            }
        }
        userHabitSelection = Regex.Replace(userHabitSelection, @"[\s]", "_");
        userHabitSelection = Regex.Replace(userHabitSelection, @"[^a-zA-Z0-9_]", "");


        if (currentHabits.Contains(userHabitSelection))
        {
            validHabitSelected = true;
        } else
        {
            Console.WriteLine("Sorry, but that didn't look like a habit you are currently tracking. Please try again.");
        }
    }

    using (SqliteConnection connection = new SqliteConnection("DataSource=Habits.db"))
    {
        connection.Open();
        habitUnitOfMeasure = ReadUnitOfMeasureFromHabit(connection,userHabitSelection);
        habitUnitOfMeasure = Regex.Replace(habitUnitOfMeasure, @"[^a-zA-Z0-9_]", "");

        for(int i = 0; i < specialtyStatQueries.Count; i++)
        {
            for (int j=0; j<specialtyStatQueries[i].Count; j++)
            {
                specialtyStatQueries[i][j] = Regex.Replace(specialtyStatQueries[i][j], "@userHabitSelection", userHabitSelection);
                specialtyStatQueries[i][j] = Regex.Replace(specialtyStatQueries[i][j], "@habitUnitOfMeasure", habitUnitOfMeasure);
            }
        }

        SqliteCommand command = connection.CreateCommand();

        foreach(List<String> specialtyQuery in specialtyStatQueries)
        {
            command.CommandText = specialtyQuery[1];
            var executed = command.ExecuteScalar();
            if (executed != null)
            {
                Console.Write(specialtyQuery[0]);
                Console.WriteLine(executed.ToString());
            } else
            {
                Console.WriteLine(specialtyQuery[0], "Unknown.");
            }
        }
        // TODO -- challenge to think about: potentially adding a feature to return longest streak of consecutive entries
        // maybe?
    }

    if (voiceMode)
    {
        Console.WriteLine("\nSay 'Continue' to continue back to the main menu.");
        while (!backToMainMenu)
        {
            userContinue = GetVoiceInput().Result;
            if (userContinue.StartsWith("c"))
            {
                backToMainMenu = true;
            }
        }
    } else
    {
        Console.WriteLine("\nPress the 'Enter' key to return back to the main menu.");
        Console.ReadLine();
    }
}

void AutoPopulateSampleData()
{
    using (SqliteConnection connection = new SqliteConnection("DataSource=Habits.db"))
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();

        command.CommandText =
            $@"
                CREATE TABLE IF NOT EXISTS sample_habit1 (
                 id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                 sample_measure INTEGER NOT NULL,
                 date_only TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS sample_habit2 (
                 id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                 sample_measure INTEGER NOT NULL,
                 date_only TEXT NOT NULL
                );
            ";
        command.ExecuteNonQuery();

        foreach (string sampleHabit in new string[] { "sample_habit1","sample_habit2" })
        {
            for (int i = 0; i<20; i++)
            {

                command.CommandText =
                    $@"
                        INSERT INTO {sampleHabit} (sample_measure, date_only)
                        VALUES (
                            ((abs(random()) % 36) + 1),
                            date('2022-01-01', '+' || abs(random() % 1085) || ' days')
                        );                           
                    ";

                command.ExecuteNonQuery();
            }
        }
    }
}

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

string ReadUnitOfMeasureFromHabit(SqliteConnection connection, string habit)
{
    string? unitOfMeasureName = "";

    SqliteCommand validateUnitOfMeasureName = connection.CreateCommand();
    validateUnitOfMeasureName.CommandText = $"PRAGMA table_info({habit});";

    using (SqliteDataReader reader = validateUnitOfMeasureName.ExecuteReader())
    {
        while (reader.Read())
        {
            if (reader[0].ToString() == "1" && reader[1] != null)
            {
                unitOfMeasureName = reader[1].ToString();
            }
        }
    }
    if (unitOfMeasureName != null)
    {
        return unitOfMeasureName;
    }
    return "";
}

void ReadRecordsFromHabit(string habit)
{
    habit = Regex.Replace(habit, @"[^a-zA-Z0-9_]", ""); // remove non-alphanumeric to reduce risk of sql injection
    string commandText = $"SELECT * FROM {habit};"; // cannot parameterize table names, must format command string manually

    Console.WriteLine($"Here are the existing records from the habit {habit}: \n");

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
                Console.WriteLine($"{reader.GetString(0)}\t{reader.GetString(1)}\t{reader.GetString(2)}");
            }
        }
    }
    Console.WriteLine("\t--------------------\n");
}

string GetUserDateInput(bool voiceMode)
{
    List<string> datePieces = new();
    int preformattedDate = -1;
    string date = "";
    string userIntAsString = "";

    Console.WriteLine("We need the date for the new record of your habit. Please enter the year of the date (yyyy)");
    datePieces.Add(GetUserIntInput(voiceMode, gettingYear: true).ToString());

    Console.WriteLine("Please enter the month of the date of your new habit record (mm)");
    preformattedDate = GetUserIntInput(voiceMode, gettingMonth: true);
    if (preformattedDate < 10)
    {
        userIntAsString = "0" + preformattedDate.ToString();
    }
    else
    {
        userIntAsString = preformattedDate.ToString();
    }

    Console.WriteLine("Please enter the DAY of the month for the date of your new habit record (dd)");
    preformattedDate = GetUserIntInput(voiceMode, gettingDay: true);
    if (preformattedDate < 10)
    {
        userIntAsString = "0" + preformattedDate.ToString();
    }
    else
    {
        userIntAsString = preformattedDate.ToString();
    }
    datePieces.Add(userIntAsString);

    datePieces.Add(userIntAsString);
    date = String.Join("-", datePieces);

    return date;
}

int GetUserIntInput(bool voiceMode, bool gettingYear=false, bool gettingDay=false, bool gettingMonth=false)
{
    bool gettingDate = false;
    string? maybeNumber = "";
    bool validNumber = false;
    int realNumber = -1;

    if (gettingYear || gettingMonth || gettingDay)
    {
        gettingDate = true;
    }

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
            if (gettingDate)
            {
                if (realNumber <= 0)
                {
                    Console.WriteLine("Dates can't be negative or zero. Please enter a positive number.");
                }

                if (gettingYear && (realNumber >3000 || realNumber <1000))
                {
                    Console.WriteLine("Wow! You must live in another age. We can only do years between 1000 and 3000. Please try again.");
                } else if (gettingDay && realNumber > 31)
                {
                    Console.WriteLine("Woah! I don't know which cool time system you're on, but our months only have days between 1 and 31. Please try again.");
                } else if (gettingMonth && realNumber >12)
                {
                    Console.WriteLine("Woah! I don't know what cool calendar you're on, but ours only has months between 1 and 12.");
                } else
                {
                    validNumber = true;
                }
            } else
            {
                validNumber =true;
            }
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
