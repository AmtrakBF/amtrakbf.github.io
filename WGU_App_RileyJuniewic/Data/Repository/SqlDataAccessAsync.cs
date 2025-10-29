using SQLite;
using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Data.Repository;

public class SqlDataAccessAsync
{
    private SQLiteAsyncConnection _connection;

    public SqlDataAccessAsync()
    {
        _connection = new SQLiteAsyncConnection("mysqliteasync.db");
        _ = InitializeAsync();
    }

    public async Task InitializeAsync()
    {
        await _connection.CreateTableAsync<Term>();
        await _connection.CreateTableAsync<Course>();
        await _connection.CreateTableAsync<Assessment>();
        await _connection.CreateTableAsync<Instructor>();
        await _connection.CreateTableAsync<Note>();
    }

    public SQLiteAsyncConnection GetConnection() => _connection;
}