using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Enums;

namespace WGU_App_RileyJuniewic.Tests.Data.Models;

public class CourseTests : BaseTest
{

    public CourseTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {
    }


    [Fact]
    public async Task CreateCourse_CreatesDBEntryAsync()
    {
        var course = Course.CreateNewInstance(Guid.NewGuid(), Guid.NewGuid(), "Test Course", CourseStatus.Active, DateTime.Now, DateTime.Now.AddDays(1));

        await _dbAccessAsync.GetConnectionAsync().InsertAsync(course);
        var courseFromDb = await _dbAccessAsync.GetConnectionAsync().GetAsync<Course>(course.CourseId);
        courseFromDb.Should().BeEquivalentTo(course);
    }   
}
