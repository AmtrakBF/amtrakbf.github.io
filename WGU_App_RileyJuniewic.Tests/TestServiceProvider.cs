using System.Reflection;
using Microsoft.Extensions.Configuration;
using WGU_App_RileyJuniewic.Data;
using WGU_App_RileyJuniewic.Data.Repository;
using WGU_App_RileyJuniewic.Data.Services;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace WGU_App_RileyJuniewic.Tests;

public class TestServiceProvider : TestBedFixture
{
    protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
    {
        var mainAppDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        using var stream = Assembly.GetExecutingAssembly()
			.GetManifestResourceStream("WGU_App_RileyJuniewic.Tests.appsettings.test.json");
		var config = new ConfigurationBuilder().AddJsonStream(stream!).Build();

        services.AddSingleton<IConfiguration>(config);

        services.AddSingleton(provider =>
        {
            var dbAccess = new SqlDataAccessAsync(config);
            dbAccess.InitializeAsync().Wait();
            return dbAccess;
        });
        
        services.AddScoped<ITermService, TermService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IInstructorService, InstructorService>();
        services.AddScoped<IAssessmentService, AssessmentService>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISearchService, SearchService>();

        services.AddSingleton<UserStore>();
    }

    protected override ValueTask DisposeAsyncCore() => new();

    protected override IEnumerable<TestAppSettings> GetTestAppSettings()
    {
        yield return new() { Filename = "appsettings.json", IsOptional = true };
        yield return new() { Filename = "appsettings.test.json", IsOptional = false };
    }
}
