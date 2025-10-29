
using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Tests.Data.Models;

public class InstructorTests : BaseTest
{
    public InstructorTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {
    }

    [Fact]
    public async Task CreateInstructor_CreatesDBEntryAsync()
    {
        var instructor = Instructor.CreateNewInstance("Test Instructor", "test@test.com", "555-555-5555");

        await _dbAccessAsync.GetConnection().InsertAsync(instructor);
        var instructorFromDb = await _dbAccessAsync.GetConnection().GetAsync<Instructor>(instructor.InstructorId);
        instructorFromDb.Should().BeEquivalentTo(instructor);
    }
}
