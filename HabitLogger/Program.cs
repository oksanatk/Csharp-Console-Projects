﻿using Microsoft.Data.Sqlite;
using Microsoft.CognitiveServices.Speech;
using System.Text.RegularExpressions;
using Spectre.Console;


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
        AnsiConsole.MarkupLine("[bold yellow]Welcome to your personal Habit Logger.[/]\n");
        AnsiConsole.MarkupLine("Please select what you'd like to do:\n");

        Panel mainMenuPanel = ShowMenuOptionsPanel(voiceMode);
        AnsiConsole.Write(mainMenuPanel);

        AnsiConsole.MarkupLine("\nOR enter [bold yellow]exit[/] to exit the application.");


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
                ViewHabitReport(currentHabits, voiceMode); 
                break;
            case "exit":
                endApp = true;
                break;

            default:
                if (voiceMode)
                {
                    AnsiConsole.MarkupLine("I'm sorry, but I didn't understand that menu option. Please try again in a few seconds.");
                    Thread.Sleep(2500);
                } else
                {
                    AnsiConsole.MarkupLine("I'm sorry, but I didn't understand that menu option. Press [bold yellow]Enter[/] to try again.");
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

    AnsiConsole.MarkupLine("\nHere are the current habits you are logging: \n");
    currentHabits.ForEach(h => AnsiConsole.MarkupLine($"[lightyellow3]{h}[/]"));
    AnsiConsole.MarkupLine("\t--------------------\n");
    while (!validHabitSelected)
    {
        if (voiceMode)
        {
            AnsiConsole.MarkupLine("\nPlease say the name of a new habit you would like to track.");
            userHabitInput = GetVoiceInput().Result;
        }
        else
        {
            AnsiConsole.MarkupLine("\nPlease type the name of a new habit you would like to track.");
            string? readResult = Console.ReadLine();
            if (readResult != null)
            {
                userHabitInput = readResult.Trim().ToLower();
            }
        }
        if (!currentHabits.Contains(userHabitInput))
        {
            validHabitSelected = true;
            AnsiConsole.MarkupLine($"\nYou are choosing to create a new habit called {userHabitInput}.");
            AnsiConsole.MarkupLine("What would you like to choose to be the unit of measure?");

            if (voiceMode)
            {
                AnsiConsole.MarkupLine($"\nPlease say the unit of measure you would like to use to track your {userHabitInput} habit.");
                userUnitOfMeasure = GetVoiceInput().Result;
                userUnitOfMeasure = Regex.Replace(userUnitOfMeasure, @"[\s]", "_");
                userUnitOfMeasure = Regex.Replace(userUnitOfMeasure, @"[^a-z0-9_]", "");
            }
            else
            {
                string? readResult = Console.ReadLine();
                if (readResult != null)
                {
                    userUnitOfMeasure = readResult.Trim().ToLower();
                    userUnitOfMeasure = Regex.Replace(userUnitOfMeasure, @"[\s]", "_");
                    userUnitOfMeasure = Regex.Replace(userUnitOfMeasure, @"[^a-z0-9_]", "");
                }
            }
            AnsiConsole.MarkupLine($"You are choosing {userUnitOfMeasure} to be the unit of measure.");
        }
        else
        {
            if (voiceMode)
            {
                AnsiConsole.MarkupLine("I'm sorry, but you already have a habit with that name. Please try again.");
            }
            else
            {
                AnsiConsole.MarkupLine("I'm sorry, but you already have a habit with that name. Please press [bold yellow]Enter[/] to try again.");
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
    AnsiConsole.MarkupLine($"New habit called {userHabitInput} with the unit of measure {userUnitOfMeasure} has been created!");
}

void EditExistingHabit(List<String> currentHabits, bool voiceMode)
{
    string userHabitInput = "";
    string userEditSelection = "";
    bool validHabitSelected = false;
    bool validAddOrEditOption = false;

    AnsiConsole.MarkupLine("\nHere are the current habits you are logging: \n");
    currentHabits.ForEach(h => AnsiConsole.MarkupLine($"[lightyellow3]{h}[/]"));
    AnsiConsole.MarkupLine("\t--------------------\n");

    // select which habit to modify, and whether user would prefer to add a new entry or update an existing one. 

    while (!validHabitSelected)
    {
        AnsiConsole.MarkupLine("Please select which of the above habits you'd like to modify.");
        if (voiceMode)
        {
            userHabitInput = GetVoiceInput().Result;
            userHabitInput = Regex.Replace(userHabitInput, @"[\s]", "_");
        }
        else
        {
            string? readResult = Console.ReadLine();
            if (readResult != null)
            {
                userHabitInput = readResult;
                userHabitInput = Regex.Replace(userHabitInput, @"[\s]", "_");
                readResult = null;
            }
        }
        if (currentHabits.Contains(userHabitInput))
        {
            validHabitSelected = true;
               
            while (!validAddOrEditOption)
            {
                AnsiConsole.MarkupLine($"\nGreat! Please select whether you'd like to [bold yellow]update[/] or [bold yellow]add[/] or [bold yellow]delete[/] a record. Or enter [bold yellow]exit[/] to return to the main menu.");
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
                        AnsiConsole.MarkupLine($"You're choosing to update an existing record.");
                        UpdateRecord(userHabitInput, voiceMode);
                        validAddOrEditOption = true;
                        break;

                    case "add":
                        AnsiConsole.MarkupLine($"You're choosing to add a new record.");
                        AddNewRecordToHabit(userHabitInput, voiceMode);
                        validAddOrEditOption = true;
                        break;

                    case "delete":
                        AnsiConsole.MarkupLine($"You're choosing to delete a record.");
                        DeleteRecord(userHabitInput, voiceMode);
                        validAddOrEditOption = true;
                        break;

                    case "exit":
                        validAddOrEditOption= true;
                        break;

                    default:
                        AnsiConsole.MarkupLine("I'm sorry, but I didn't understand that. Please try again.");
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
        AnsiConsole.MarkupLine("Here's a list of the current habits you are tracking.");
        currentHabits.ForEach(x => AnsiConsole.MarkupLine($"[lightyellow3]{x}[/]"));
        AnsiConsole.MarkupLine("\t--------------------\n");
        
        AnsiConsole.MarkupLine("Please enter the habit you would like to delete. Or enter [bold yellow]exit[/] to exit back to the main screen.");

        if (voiceMode)
        {
            userSelectedHabit = GetVoiceInput().Result;
            userSelectedHabit = Regex.Replace(userSelectedHabit, @"[\s]", "_");
        } else
        {
            readResult = Console.ReadLine();
            if (readResult != null)
            {
                userSelectedHabit = readResult.Trim().ToLower();
                userSelectedHabit = Regex.Replace(userSelectedHabit, @"[\s]", "_");
                readResult = null;
            }
        }

        if (userSelectedHabit != null && currentHabits.Contains(userSelectedHabit))
        {
            AnsiConsole.MarkupLine($"You are choosing to delete the habit {userSelectedHabit}. Please confirm if you would like to continue (y/n).");
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

        AnsiConsole.MarkupLine($"Your habit {userSelectedHabit} has been successfully deleted.");
        do
        {
            if (voiceMode)
            {            
                AnsiConsole.MarkupLine("Say [bold yellow]continue[/] to continue back to the main screen.");
                continueConfirmation = GetVoiceInput().Result;
                if (continueConfirmation.StartsWith("c"))
                {
                    validContinue = true;
                }
            } else
            {
                AnsiConsole.MarkupLine("Press the [bold yellow]Enter[/] key to continue back to the main menu.");
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

    AnsiConsole.Write(ReadRecordsFromHabit(habit));

    AnsiConsole.MarkupLine("\t--------------------\n");
    AnsiConsole.MarkupLine("Please select the id (#) of the habit you would like to delete.");
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
        AnsiConsole.MarkupLine($"Your record for the habit {habit}, and the id {userSelectedId} is no more. Please wait a few seconds to continue.");
        Thread.Sleep(3000);
    } else
    {
        AnsiConsole.MarkupLine($"Your record for the habit {habit}, and the id {userSelectedId} is no more. Press the [bold yellow]Enter[/] key to continue.");
        Console.ReadLine();
    }
}

void UpdateRecord(string habit, bool voiceMode)
{
    int userSelectedId = -1;
    int userSelectedQuantity = -1;
    string userSelectedDate = "";
    string habitUnitOfMeasure = "";
    string confirmationMessage = "";
    bool exitToMainMenu = false;

    habit = habit.Trim().ToLower();
    habit = Regex.Replace(habit, @"[^a-zA-Z0-9_]", "");

    habitUnitOfMeasure = ReadUnitOfMeasureWithoutConnection(habit);

    AnsiConsole.Write(ReadRecordsFromHabit(habit));
    AnsiConsole.MarkupLine("\t--------------------\n");

    AnsiConsole.MarkupLine("Please select the id (#) of the record you would like to update.");
    userSelectedId = GetUserIntInput(voiceMode);

    AnsiConsole.MarkupLine($"Now we'll need to get the new date you would like to update entry with id #{userSelectedId} to.");
    userSelectedDate = GetUserDateInput(voiceMode);

    AnsiConsole.MarkupLine($"Please enter the new quantity (##) you would like to record.");
    userSelectedQuantity = GetUserIntInput(voiceMode);

    AnsiConsole.MarkupLine($"You're choosing to update the entry in the habit [yellow]{habit}[/] and id #[yellow]{userSelectedId}[/] to [yellow]{userSelectedQuantity} {habitUnitOfMeasure}[/] on [yellow]{userSelectedDate}[/]. Please confirm (y/n).");
    if (voiceMode)
    {
        confirmationMessage = GetVoiceInput().Result;
    }
    else
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

            habitUnitOfMeasure = ReadUnitOfMeasureFromHabit(connection, habit);
            habitUnitOfMeasure = habitUnitOfMeasure.Trim().ToLower();
            habitUnitOfMeasure = Regex.Replace(habitUnitOfMeasure, @"[^a-zA-Z0-9_]", "");

            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
                $@"
                INSERT INTO {habit} (id, {habitUnitOfMeasure}, date_only)
                VALUES (@userSelectedId, @userSelectedQuantity, @userSelectedDate)
                    ON CONFLICT(id) DO UPDATE SET
                    {habitUnitOfMeasure} = excluded.{habitUnitOfMeasure},
                    date_only = excluded.date_only;
            ";
            command.Parameters.AddWithValue("@userSelectedId", userSelectedId);
            command.Parameters.AddWithValue("@userSelectedQuantity", userSelectedQuantity);
            command.Parameters.AddWithValue("@userSelectedDate", userSelectedDate);

            command.ExecuteNonQuery();
        } 
    }
}

void AddNewRecordToHabit(string habit,bool voiceMode)
{
    bool exitToMainMenu = false;
    string userDateInputted = "";
    int userQuantity = -1;

    habit = Regex.Replace(habit, @"[\s]", "_");
    habit = Regex.Replace(habit, @"[^a-zA-Z0-9_]", "");

    AnsiConsole.Write(ReadRecordsFromHabit(habit));
    AnsiConsole.MarkupLine("\t--------------------\n");

    AnsiConsole.MarkupLine("To add a new entry, we'll first need the date of the new entry.\n");
    userDateInputted = GetUserDateInput(voiceMode);

    AnsiConsole.MarkupLine("Please enter the quantity to record");
    userQuantity = GetUserIntInput(voiceMode);
    
    // confirm or exit to main menu
    string confirmationMessage = "";
    AnsiConsole.MarkupLine($"You're choosing to add a new record to the habit {habit} with the quantity {userQuantity} and date {userDateInputted} (yyyy-mm-dd). Please confirm (y/n).");
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

    Grid grid = new();
    grid.AddColumns(2);
    Table statsTable = new Table();
    statsTable.AddColumn("[lightyellow3]Statistic [/]");
    statsTable.AddColumn("[lightyellow3]Value [/]");
    Table recordsTable;

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
    AnsiConsole.MarkupLine("\n\tHere are the current habits you can view a report for: \n");
    currentHabits.ForEach(h => AnsiConsole.MarkupLine($"[lightyellow3]{h}[/]"));
    AnsiConsole.MarkupLine("\t--------------------\n");

    while (!validHabitSelected)
    {
        AnsiConsole.MarkupLine("Please select the habit you would like to view statistics for.");
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
            AnsiConsole.MarkupLine("Sorry, but that didn't look like a habit you are currently tracking. Please try again.");
        }
    }

    recordsTable = ReadRecordsFromHabit(userHabitSelection);

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
                statsTable.AddRow(specialtyQuery[0], executed.ToString());
            } else
            {
                statsTable.AddRow(specialtyQuery[0], "Uknown.");
            }
        }
    }
    grid.AddRow(recordsTable, statsTable);
    AnsiConsole.Write(grid);

    if (voiceMode)
    {
        AnsiConsole.MarkupLine("\nSay [bold yellow]Continue[/] to continue back to the main menu.");
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
        AnsiConsole.MarkupLine("\nPress the [bold yellow]Enter[/] key to return back to the main menu.");
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
                CREATE TABLE IF NOT EXISTS sample_habit_1 (
                 id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                 sample_measure INTEGER NOT NULL,
                 date_only TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS sample_habit_2 (
                 id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                 sample_measure INTEGER NOT NULL,
                 date_only TEXT NOT NULL
                );
            ";
        command.ExecuteNonQuery();

        foreach (string sampleHabit in new string[] { "sample_habit_1","sample_habit_2" })
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

string ReadUnitOfMeasureWithoutConnection(string habit)
    // how to overload method without class declaration?
{
    string? unitOfMeasureName = "";
    using (SqliteConnection connection = new SqliteConnection("DataSource=Habits.db"))
    {
        connection.Open();
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
    }
    return "";
}

Table ReadRecordsFromHabit(string habit)
{
    Table table = new();
    table.Border(TableBorder.Rounded);

    habit = Regex.Replace(habit, @"[^a-zA-Z0-9_]", ""); // remove non-alphanumeric to reduce risk of sql injection
    string commandText = $"SELECT * FROM {habit};"; // cannot parameterize table names, must format command string manually

    AnsiConsole.MarkupLine($"\nHere are the existing records from the habit {habit}:");

    using (var connection = new SqliteConnection("DataSource=Habits.db"))
    {
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = commandText;

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                table.AddColumn($"[lightyellow3]{reader.GetName(i)}[/]");
            }
            AnsiConsole.MarkupLine("\n");
            while (reader.Read())
            {         
                table.AddRow(reader.GetString(0),reader.GetString(1), reader.GetString(2));
            }
        }
    }
    return table;
}

string GetUserDateInput(bool voiceMode)
{
    List<string> datePieces = new();
    int preformattedDate = -1;
    string date = "";
    string userIntAsString = "";

    AnsiConsole.MarkupLine("We need the date for the new record of your habit. Please enter the year of the date (yyyy)");
    datePieces.Add(GetUserIntInput(voiceMode, gettingYear: true).ToString());

    AnsiConsole.MarkupLine("Please enter the month of the date of your new habit record (mm)");
    preformattedDate = GetUserIntInput(voiceMode, gettingMonth: true);
    if (preformattedDate < 10)
    {
        userIntAsString = "0" + preformattedDate.ToString();
    }
    else
    {
        userIntAsString = preformattedDate.ToString();
    }

    AnsiConsole.MarkupLine("Please enter the DAY of the month for the date of your new habit record (dd)");
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
                    AnsiConsole.MarkupLine("Dates can't be negative or zero. Please enter a positive number.");
                }

                if (gettingYear && (realNumber >3000 || realNumber <1000))
                {
                    AnsiConsole.MarkupLine("Wow! You must live in another age. We can only do years between 1000 and 3000. Please try again.");
                } else if (gettingDay && realNumber > 31)
                {
                    AnsiConsole.MarkupLine("Woah! I don't know which cool time system you're on, but our months only have days between 1 and 31. Please try again.");
                } else if (gettingMonth && realNumber >12)
                {
                    AnsiConsole.MarkupLine("Woah! I don't know what cool calendar you're on, but ours only has months between 1 and 12.");
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
            AnsiConsole.MarkupLine("I'm sorry, but I didn't understand that number. Please try again.");
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
            AnsiConsole.MarkupLine("  Speech Input Recognized: {userVoiceInput}");

            userVoiceInput = userVoiceInput.Trim().ToLower();
            Regex.Replace(userVoiceInput, @"[^a-z0-9\s]", "");

            return userVoiceInput;

        } else if (result.Reason == ResultReason.Canceled)
        {
            AnsiConsole.MarkupLine($"An error occured during speech recognition: \n\t{CancellationDetails.FromResult(result)}");
        } else
        {
            if (repeatCounter <1)
            {
                AnsiConsole.MarkupLine("I'm sorry, but I didn't understand what you said. Please try again.");
            }
            repeatCounter++;
        }
    } while (result.Reason != ResultReason.RecognizedSpeech);

    return "UnexpectedVoiceResult Error";
}

Panel ShowMenuOptionsPanel(bool voiceMode)
{
    Grid grid = new Grid();
    grid.AddColumn();

    grid.AddRow("[aqua]Create[/] - Create a new habit");
    grid.AddRow("[aqua]Edit[/] - Edit or Update an existing habit");
    grid.AddRow("[aqua]Delete[/] - delete all data relating to a habit.");
    grid.AddRow("[aqua]Report[/] - view statistics about all habits.");

    return new Panel(grid)
    {
        Header = new PanelHeader("[yellow]Choose an Option:[/]"),
        Border = BoxBorder.Rounded,
        Padding = new Padding(1)
    };
}
