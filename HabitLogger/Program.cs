using Microsoft.Data.Sqlite;

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

Console.Write("user ID: ");
var id = int.Parse(Console.ReadLine());


// this is now the reader / selector? 
// maybe would make more sense to put this into a separate method ReadFromSqliteDatabase(string variable, var value)   ??

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

            Console.WriteLine($"Hello, {name}!");
        }
    }
    Console.WriteLine("\n code completed, press any key to continue.");
    Console.ReadKey();
}
    File.Delete("testCode.db");