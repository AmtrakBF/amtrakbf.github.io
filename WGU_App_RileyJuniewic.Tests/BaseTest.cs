using WGU_App_RileyJuniewic.Data.Repository;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Microsoft.DependencyInjection.Attributes;

namespace WGU_App_RileyJuniewic.Tests;

public class BaseTest : TestBedWithDI<TestServiceProvider>
{
    [Inject] protected SqlDataAccessAsync _dbAccessAsync { get; set; } = null!;

    public BaseTest(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {
    }

}
