using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Enums;

namespace WGU_App_RileyJuniewic.Tests.Data.Models;

public class AssessmentTests : BaseTest
{
    public AssessmentTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {
    }

    [Fact]
    public async Task CreateAssessment_CreatesDBEntryAsync()
    {
        var assessment = Assessment.CreateNewInstance(Guid.NewGuid(), "Test Assessment", AssessmentType.Objective, DateTime.Now, DateTime.Now.AddDays(1));

        await _dbAccessAsync.GetConnectionAsync().InsertAsync(assessment);
        var assessmentFromDb = await _dbAccessAsync.GetConnectionAsync().GetAsync<Assessment>(assessment.AssessmentId);
        assessmentFromDb.Should().BeEquivalentTo(assessment);
    }
}
