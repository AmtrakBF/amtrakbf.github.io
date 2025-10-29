using Microsoft.Extensions.Configuration;
using SQLite;
using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Data.Repository;

public class SqlDataAccessAsync
{
    private SQLiteAsyncConnection _connection;

    public SqlDataAccessAsync(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        _connection = new SQLiteAsyncConnection(connectionString);
    }

    public async Task InitializeAsync()
    {
        await _connection.CreateTableAsync<Term>();
        await _connection.CreateTableAsync<Course>();
        await _connection.CreateTableAsync<Assessment>();
        await _connection.CreateTableAsync<Instructor>();
        await _connection.CreateTableAsync<Note>();
    }

    public static async Task<SqlDataAccessAsync> CreateAndInitializeAsync(IConfiguration configuration)
    {
        var dbAccess = new SqlDataAccessAsync(configuration);
        await dbAccess.InitializeAsync();
        return dbAccess;
    }

    public SQLiteAsyncConnection GetConnection() => _connection;
}