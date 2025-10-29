using Microsoft.Extensions.Configuration;
using WGU_App_RileyJuniewic.Data.Repository;
using WGU_App_RileyJuniewic.Data.Services;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace WGU_App_RileyJuniewic.Tests;

public class TestServiceProvider : TestBedFixture
{
    protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
        => services
            .AddSingleton<SqlDataAccessAsync>()
            .AddScoped<ITermService, TermService>()
            .AddScoped<ICourseService, CourseService>();

    protected override ValueTask DisposeAsyncCore() => new();

    protected override IEnumerable<TestAppSettings> GetTestAppSettings()
    {
        yield return new() { Filename = "appsettings.json", IsOptional = true };
    }
}
