using Microsoft.Extensions.Configuration;
using SQLite;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Enums;

namespace WGU_App_RileyJuniewic.Data.Repository;

public class SqlDataAccessAsync
{
    private SQLiteAsyncConnection _connection;

    public SqlDataAccessAsync(IConfiguration configuration)
    {
        var connectionString =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            configuration.GetConnectionString("Default") ?? "mysqliteasync.db");

        _connection = new SQLiteAsyncConnection(connectionString);
    }

    public async Task InitializeAsync()
    {
        await _connection.CreateTableAsync<Term>();
        await _connection.CreateTableAsync<Course>();
        await _connection.CreateTableAsync<Assessment>();
        await _connection.CreateTableAsync<Instructor>();
        await _connection.CreateTableAsync<Note>();

        var instructor = new Instructor
        {
            InstructorId = Guid.NewGuid(),
            Name = "Anika Patel",
            Email = "anika.patel@strimeuniversity.edu",
            Phone = "555-123-4567"
        };
        await _connection.InsertAsync(instructor);

        var term = new Term
        {
            TermId = Guid.NewGuid(),
            Title = "Summer 2025",
            StartDate = new DateTime(2025, 5, 1),
            EndDate = new DateTime(2025, 11, 30)
        };
        await _connection.InsertAsync(term);

        var course = new Course
        {
            CourseId = Guid.NewGuid(),
            Title = "C971 Mobile Application Development Using C#",
            StartDate = new DateTime(2025, 10, 25),
            EndDate = new DateTime(2025, 11, 9),
            Status = CourseStatus.Active,
            TermId = term.TermId,
            InstructorId = instructor.InstructorId
        };
        await _connection.InsertAsync(course);
    }

    public static async Task<SqlDataAccessAsync> CreateAndInitializeAsync(IConfiguration configuration)
    {
        var dbAccess = new SqlDataAccessAsync(configuration);
        await dbAccess.InitializeAsync();
        return dbAccess;
    }

    public SQLiteAsyncConnection GetConnection() => _connection;
}