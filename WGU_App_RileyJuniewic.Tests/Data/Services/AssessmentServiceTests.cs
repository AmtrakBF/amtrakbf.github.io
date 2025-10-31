using Ardalis.Result;
using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Dtos.Assessment;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Enums;
using WGU_App_RileyJuniewic.Data.Repository;
using WGU_App_RileyJuniewic.Data.Services;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Microsoft.DependencyInjection.Attributes;

namespace WGU_App_RileyJuniewic.Tests.Data.Services;

public class AssessmentServiceTests : TestBedWithDI<TestServiceProvider>
{
    [Inject] public IAssessmentService _assessmentService { get; set; } = null!;
    [Inject] public SqlDataAccessAsync _sqlDataAccess { get; set; } = null!;

    public AssessmentServiceTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {
    }

    [Fact]
    public async Task CreateAssessmentAsync_CreatesAssessmentAsync()
    {
        await ClearAssessments();

        var course = new Course() { CourseId = Guid.NewGuid() };
        await _sqlDataAccess.GetConnection().InsertAsync(course);

        var assessmentRequest = new CreateAssessmentRequest()
        {
            CourseId = course.CourseId,
            Name = "Assessment 1",
            Type = AssessmentType.Objective,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var result = await _assessmentService.CreateAssessmentAsync(assessmentRequest);
        var dbAssessment = await _sqlDataAccess.GetConnection().GetAsync<Assessment>(result.Value.AssessmentId);
        dbAssessment.Should().BeEquivalentTo(result.Value);
    }

    [Fact]
    public async Task CreateAssessmentAsync_ThrowsExceptionWhenCourseDoesNotExistAsync()
    {
        await ClearAssessments();

        var course = new Course() { CourseId = Guid.NewGuid() };
        await _sqlDataAccess.GetConnection().InsertAsync(course);

        var assessmentRequest = new CreateAssessmentRequest()
        {
            CourseId = Guid.NewGuid(),
            Name = "Assessment 1",
            Type = AssessmentType.Objective,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var result = await _assessmentService.CreateAssessmentAsync(assessmentRequest);
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("Course does not exist");
    }

    [Fact]
    public async Task CreateAssessmentAsync_ThrowsExceptionWhenAssessmentNameAlreadyExistsAsync()
    {
        await ClearAssessments();

        var course = new Course() { CourseId = Guid.NewGuid() };
        await _sqlDataAccess.GetConnection().InsertAsync(course);

        var assessmentRequest = new CreateAssessmentRequest()
        {
            CourseId = course.CourseId,
            Name = "Assessment 1",
            Type = AssessmentType.Objective,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        await _assessmentService.CreateAssessmentAsync(assessmentRequest);

        var assessmentRequest2 = new CreateAssessmentRequest()
        {
            CourseId = course.CourseId,
            Name = "Assessment 1",
            Type = AssessmentType.Objective,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var result = await _assessmentService.CreateAssessmentAsync(assessmentRequest2);
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("Assessment name already exists");
    }

    [Fact]
    public async Task CreateAssessmentAsync_ThrowsExceptionWhenAssessmentOverlapsWithExistingAssessmentAsync()
    {
        await ClearAssessments();

        var course = new Course() { CourseId = Guid.NewGuid() };
        await _sqlDataAccess.GetConnection().InsertAsync(course);

        var assessmentRequest = new CreateAssessmentRequest()
        {
            CourseId = course.CourseId,
            Name = "Assessment 1",
            Type = AssessmentType.Objective,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        await _assessmentService.CreateAssessmentAsync(assessmentRequest);

        var assessmentRequest2 = new CreateAssessmentRequest()
        {
            CourseId = course.CourseId,
            Name = "Assessment 2",
            Type = AssessmentType.Objective,
            StartDate = DateTime.Now.AddDays(0.5),
            EndDate = DateTime.Now.AddDays(1.5)
        };

        var result = await _assessmentService.CreateAssessmentAsync(assessmentRequest2);
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("Assessment overlaps with existing assessment");
    }

    [Fact]
    public async Task DeleteAssessmentAsync_DeletesAssessmentAsync()
    {
        await ClearAssessments();

        var course = new Course() { CourseId = Guid.NewGuid() };
        await _sqlDataAccess.GetConnection().InsertAsync(course);

        var assessmentRequest = new CreateAssessmentRequest()
        {
            CourseId = course.CourseId,
            Name = "Assessment 1",
            Type = AssessmentType.Objective,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var result = await _assessmentService.CreateAssessmentAsync(assessmentRequest);
        await _assessmentService.DeleteAssessmentAsync(result.Value.AssessmentId);
        
        var dbAssessment = await _sqlDataAccess.GetConnection().Table<Assessment>().Where(x => x.AssessmentId == result.Value.AssessmentId).FirstOrDefaultAsync();
        dbAssessment.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAssessmentsAsync_ReturnsAllAssessmentsAsync()
    {
        await ClearAssessments();

        var course = new Course() { CourseId = Guid.NewGuid() };
        await _sqlDataAccess.GetConnection().InsertAsync(course);

        var assessmentRequest = new CreateAssessmentRequest()
        {
            CourseId = course.CourseId,
            Name = "Assessment 1",
            Type = AssessmentType.Objective,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var assessmentRequest2 = new CreateAssessmentRequest()
        {
            CourseId = course.CourseId,
            Name = "Assessment 2",
            Type = AssessmentType.Objective,
            StartDate = DateTime.Now.AddDays(2),
            EndDate = DateTime.Now.AddDays(3)
        };

        var result = await _assessmentService.CreateAssessmentAsync(assessmentRequest);
        var result2 = await _assessmentService.CreateAssessmentAsync(assessmentRequest2);

        var assessments = await _assessmentService.GetAllAssessmentsAsync();
        assessments.Should().ContainEquivalentOf(result.Value);
        assessments.Should().ContainEquivalentOf(result2.Value);
    }

    [Fact]
    public async Task GetAssessmentAsync_ReturnsAssessmentAsync()
    {
        await ClearAssessments();

        var course = new Course() { CourseId = Guid.NewGuid() };
        await _sqlDataAccess.GetConnection().InsertAsync(course);

        var assessmentRequest = new CreateAssessmentRequest()
        {
            CourseId = course.CourseId,
            Name = "Assessment 1",
            Type = AssessmentType.Objective,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var result = await _assessmentService.CreateAssessmentAsync(assessmentRequest);
        var assessment = await _assessmentService.GetAssessmentAsync(result.Value.AssessmentId);
        assessment.Should().BeEquivalentTo(result);
    }

    [Fact]
    public async Task GetAssessmentAsync_ThrowsExceptionWhenAssessmentDoesNotExistAsync()
    {
        await ClearAssessments();

        var result = await _assessmentService.GetAssessmentAsync(Guid.NewGuid());
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("Assessment not found");
    }
    
    [Fact]
    public async Task UpdateAssessmentAsync_UpdatesAssessmentAsync()
    {
        await ClearAssessments();

        var course = new Course() { CourseId = Guid.NewGuid() };
        await _sqlDataAccess.GetConnection().InsertAsync(course);

        var assessmentRequest = new CreateAssessmentRequest()
        {
            CourseId = course.CourseId,
            Name = "Assessment 1",
            Type = AssessmentType.Objective,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };
        
        var result = await _assessmentService.CreateAssessmentAsync(assessmentRequest);
        var updateRequest = new UpdateAssessmentRequest()
        {
            AssessmentId = result.Value.AssessmentId,
            CourseId = result.Value.CourseId,
            Name = "Updated Assessment",
            Type = AssessmentType.Objective,
            StartDate = DateTime.Now.AddDays(2),
            EndDate = DateTime.Now.AddDays(3)
        };

        var updatedAssessmentResult = await _assessmentService.UpdateAssessmentAsync(updateRequest);
        var updatedAssessment = updatedAssessmentResult.Value;
        updatedAssessment.AssessmentId.Should().Be(updatedAssessment.AssessmentId);
        updatedAssessment.CourseId.Should().Be(updatedAssessment.CourseId);
        updatedAssessment.Name.Should().Be(updatedAssessment.Name);
        updatedAssessment.Type.Should().Be(updatedAssessment.Type);
        updatedAssessment.StartDate.Should().Be(updatedAssessment.StartDate);
        updatedAssessment.EndDate.Should().Be(updatedAssessment.EndDate);
    }


    private async Task ClearAssessments() => await _sqlDataAccess.GetConnection().DeleteAllAsync<Assessment>();
}