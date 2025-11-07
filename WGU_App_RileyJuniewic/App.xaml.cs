using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Enums;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic;

public partial class App : Application
{
    private readonly SqlDataAccessAsync _sqlDataAccess;

    public App(SqlDataAccessAsync sqlDataAccessAsync)
	{
		InitializeComponent();

		_sqlDataAccess = sqlDataAccessAsync;
		_ = InitDB();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	private async Task InitDB()
	{
		var connection = await _sqlDataAccess.GetConnectionAsync();
		var isInitialized = await connection.Table<InitDB>().FirstOrDefaultAsync() ?? new InitDB();
		if (isInitialized.IsInitialized)
			return;

		var instructor = new Instructor
        {
            InstructorId =  Guid.NewGuid(),
            Name = "Anika Patel",
            Email = "anika.patel@strimeuniversity.edu",
            Phone = "555-123-4567"
        };
        await connection.InsertAsync(instructor);

        var term = new Term
        {
            TermId = Guid.NewGuid(),
            Title = "Summer 2025",
            StartDate = new DateTime(2025, 5, 1),
            EndDate = new DateTime(2025, 11, 30)
        };
        await connection.InsertAsync(term);

        var course = new Course
        {
            CourseId = Guid.NewGuid(),
            Title = "C971 Mobile Application Development Using C#",
            StartDate = new DateTime(2025, 10, 25),
            EndDate = new DateTime(2025, 11, 9),
            Status = CourseStatus.Completed,
            TermId = term.TermId,
            InstructorId = instructor.InstructorId
        };
		await connection.InsertAsync(course);
		
		await connection.InsertAsync(new InitDB
		{
			IsInitialized = true
		});
    }
}