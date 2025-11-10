using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Enums;
using WGU_App_RileyJuniewic.Data.Repository;
using WGU_App_RileyJuniewic.Data.Services;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Microsoft.DependencyInjection.Attributes;

namespace WGU_App_RileyJuniewic.Tests.Data.Services;

public class SearchServiceTests : TestBedWithDI<TestServiceProvider>
{
    [Inject] public ISearchService _searchService { get; set; } = null!;
    [Inject] public SqlDataAccessAsync _sqlDataAccess { get; set; } = null!;

    private Task _init;

    public SearchServiceTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {
        _init = InitDB();
    }

    private async Task InitDB()
    {
        var connection = await _sqlDataAccess.GetConnectionAsync();
        await connection.DeleteAllAsync<Assessment>();
        await connection.DeleteAllAsync<Course>();
        await connection.DeleteAllAsync<Term>();
        await connection.DeleteAllAsync<Instructor>();
        await connection.DeleteAllAsync<User>();

        var user = new User() { UserId = new Guid("84c9e24e-742a-41ee-9025-35dfbba98e20"), Username = "test", Password = "test" };
        var term = new Term() { TermId = Guid.NewGuid(), UserId = user.UserId, Title = "Test Term", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
        var instructor = new Instructor() { InstructorId = Guid.NewGuid(), UserId = user.UserId, Name = "Test Instructor", Email = "test@test.com", Phone = "1234567890" };
        var course = new Course() { CourseId = Guid.NewGuid(), InstructorId = instructor.InstructorId, TermId = term.TermId, Title = "Test Course", Notes = "Test Notes", Status = CourseStatus.Active };
        var assessment = new Assessment() { AssessmentId = Guid.NewGuid(), CourseId = course.CourseId, Name = "Test Assessment", Type = AssessmentType.Objective, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
        await connection.InsertAsync(user);
        await connection.InsertAsync(term);
        await connection.InsertAsync(course);
        await connection.InsertAsync(instructor);
        await connection.InsertAsync(assessment);
        var assessment2 = new Assessment() { AssessmentId = Guid.NewGuid(), CourseId = course.CourseId, Name = "Test TEST", Type = AssessmentType.Objective, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
        await connection.InsertAsync(assessment2);
    }

    [Fact]
    public async Task SearchAssessmentsAsync_ReturnsAssessmentsAsync()
    {
        await _init;

        var searchResults = await _searchService.SearchAllAsync("ass", new Guid("84c9e24e-742a-41ee-9025-35dfbba98e20"));
        searchResults.Should().NotBeNull();
        searchResults.Assessments.Should().NotBeNull();
        searchResults.Assessments.Count().Should().Be(1);
        searchResults.Assessments.First().Name.Should().Be("Test Assessment");

        searchResults.Terms.Should().BeEmpty();
        searchResults.Courses.Should().BeEmpty();
        searchResults.Instructors.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchCourseAsync_ReturnsCoursesAsync()
    {
        await _init;

        var searchResults = await _searchService.SearchAllAsync("Course", new Guid("84c9e24e-742a-41ee-9025-35dfbba98e20"));
        searchResults.Should().NotBeNull();
        searchResults.Courses.Count().Should().Be(1);
        searchResults.Courses.First().Title.Should().Be("Test Course");

        searchResults.Assessments.Should().BeEmpty();
        searchResults.Terms.Should().BeEmpty();
        searchResults.Instructors.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchTermAsync_ReturnsTermsAsync()
    {
        await _init;

        var searchResults = await _searchService.SearchAllAsync("Term", new Guid("84c9e24e-742a-41ee-9025-35dfbba98e20"));
        searchResults.Should().NotBeNull();
        searchResults.Terms.Count().Should().Be(1);
        searchResults.Terms.First().Title.Should().Be("Test Term");

        searchResults.Assessments.Should().BeEmpty();
        searchResults.Courses.Should().BeEmpty();
        searchResults.Instructors.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchInstructorAsync_ReturnsInstructorsAsync()
    {
        await _init;

        var searchResults = await _searchService.SearchAllAsync("Instructor", new Guid("84c9e24e-742a-41ee-9025-35dfbba98e20"));
        searchResults.Should().NotBeNull();
        searchResults.Instructors.Count().Should().Be(1);
        searchResults.Instructors.First().Name.Should().Be("Test Instructor");

        searchResults.Assessments.Should().BeEmpty();
        searchResults.Courses.Should().BeEmpty();
        searchResults.Terms.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchAllAsync_ReturnsAllAsync()
    {
        await _init;

        var searchResults = await _searchService.SearchAllAsync("Test", new Guid("84c9e24e-742a-41ee-9025-35dfbba98e20"));
        searchResults.Should().NotBeNull();
        searchResults.Instructors.Should().NotBeEmpty();
        searchResults.Assessments.Should().NotBeEmpty();
        searchResults.Courses.Should().NotBeEmpty();
        searchResults.Terms.Should().NotBeEmpty();
    }

}
