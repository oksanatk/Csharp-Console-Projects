using SQLite;
using TCSA.MathGame.Maui.Models;
namespace TCSA.MathGame.Maui.Data;

public class GameRepository
{
    string _dbPath;
    private SQLiteConnection connection;

    public GameRepository(string dbPath)
    {
        _dbPath = dbPath;
        Init();
    }

    private void Init()
    {
        connection = new SQLiteConnection( _dbPath );
        connection.CreateTable<Game>();
    }

    public List<Game> GetAllGames()
    {
        Init();
        return connection.Table<Game>().ToList();
    }

    public void Add( Game game)
    {
        connection = new SQLiteConnection(_dbPath);
        connection.Insert( game );
    }

    public void Delete(int id)
    {
        connection = new SQLiteConnection(_dbPath);
        connection.Delete(new Game { Id = id });
    }
}

