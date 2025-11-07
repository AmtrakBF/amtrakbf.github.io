using Microsoft.Extensions.Configuration;
using SQLite;
using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Data.Repository;

public class SqlDataAccessAsync
{
    private SQLiteAsyncConnection _connection;
    private bool _isInitialized = false;

    public SqlDataAccessAsync(IConfiguration configuration)
    {
        var connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            configuration.GetConnectionString("Default") ?? "mysqliteasync.db");

        _connection = new SQLiteAsyncConnection(connectionString);
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized)
            return;

        await _connection.CreateTableAsync<Term>();
        await _connection.CreateTableAsync<Course>();
        await _connection.CreateTableAsync<Assessment>();
        await _connection.CreateTableAsync<Instructor>();
        await _connection.CreateTableAsync<Note>();
        await _connection.CreateTableAsync<InitDB>();

        _isInitialized = true;
    }

    public async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (!_isInitialized)
            await InitializeAsync();

        return _connection;
    }
}