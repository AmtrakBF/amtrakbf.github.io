using Ardalis.Result;
using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Enums;
using WGU_App_RileyJuniewic.Data.Repository;
using WGU_App_RileyJuniewic.Data.Services;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Microsoft.DependencyInjection.Attributes;

namespace WGU_App_RileyJuniewic.Tests.Data.Services;

public class CourseServiceTests : TestBedWithDI<TestServiceProvider>
{

    [Inject] protected ICourseService _courseService { get; set; } = null!;
    [Inject] protected SqlDataAccessAsync _dbAccessAsync { get; set; } = null!;

    public CourseServiceTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {
    }

    [Fact]
    public async Task CreateCourseAsync_CreatesDBEntryAsync()
    {
        await ClearCourses();

        var (instructor, term) = await CreateInstructorAndTermAsync();

        var course = new CreateCourseRequest()
        {
            Title = "Test Course",
            TermId = term.TermId,
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.InProgress.ToString(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var createdCourse = await _courseService.CreateCourseAsync(course);
        var courseFromDb = await _courseService.GetCourseAsync(createdCourse.Value.CourseId);

        courseFromDb.Should().BeEquivalentTo(createdCourse);
    }

    // [Fact]
    // public async Task CreateCourseAsync_ThrowsExceptionWhenCourseOverlapsAsync()
    // {
    //     await ClearCourses();

    //     var (instructor, term) = await CreateInstructorAndTermAsync();

    //     var course = new CreateCourseRequest()
    //     {
    //         Title = "Test Course",
    //         TermId = term.TermId,
    //         InstructorId = instructor.InstructorId,
    //         Status = CourseStatus.InProgress.ToString(),
    //         StartDate = new DateTime(2023, 1, 1),
    //         EndDate = new DateTime(2023, 2, 2)
    //     };

    //     await _courseService.CreateCourseAsync(course);

    //     var course2 = new CreateCourseRequest()
    //     {
    //         Title = "Test Course2",
    //         TermId = term.TermId,
    //         InstructorId = instructor.InstructorId,
    //         Status = CourseStatus.InProgress.ToString(),
    //         StartDate = new DateTime(2023, 2, 2),
    //         EndDate = new DateTime(2023, 3, 3)
    //     };

    //     var result = await _courseService.CreateCourseAsync(course2);
    //     result.IsError().Should().BeTrue();
    //     result.Errors.First().Should().Be("Course overlaps with an existing course");
    // }

    [Fact]
    public async Task CreateCourseAsync_Creates_WhenCourseOverlapsAsync()
    {
        await ClearCourses();

        var (instructor, term) = await CreateInstructorAndTermAsync();

        var course = new CreateCourseRequest()
        {
            Title = "Test Course",
            TermId = term.TermId,
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.InProgress.ToString(),
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 2, 2)
        };

        await _courseService.CreateCourseAsync(course);

        var course2 = new CreateCourseRequest()
        {
            Title = "Test Course2",
            TermId = term.TermId,
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.InProgress.ToString(),
            StartDate = new DateTime(2023, 2, 2),
            EndDate = new DateTime(2023, 3, 3)
        };

        var result = await _courseService.CreateCourseAsync(course2);
        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("Test Course2");
        result.Value.TermId.Should().Be(term.TermId);
        result.Value.InstructorId.Should().Be(instructor.InstructorId);
        result.Value.Status.Should().Be(CourseStatus.InProgress);
    }

    [Fact]
    public async Task CreateCourseAsync_ReturnsErrorWhenCourseTitleExistsAsync()
    {
        await ClearCourses();

        var (instructor, term) = await CreateInstructorAndTermAsync();

        var course = new CreateCourseRequest()
        {
            Title = "Test Course",
            TermId = term.TermId,
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.InProgress.ToString(),
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 2, 2)
        };

        await _courseService.CreateCourseAsync(course);

        var course2 = new CreateCourseRequest()
        {
            Title = "Test Course",
            TermId = term.TermId,
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.InProgress.ToString(),
            StartDate = new DateTime(2023, 2, 2),
            EndDate = new DateTime(2023, 3, 3)
        };

        var result = await _courseService.CreateCourseAsync(course2);
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("Course title already exists");
    }

    [Fact]
    public async Task CreateCourseAsync_ThrowsExceptionWhenInstructorDoesNotExistsAsync()
    {
        await ClearCourses();
        var (_, term) = await CreateInstructorAndTermAsync();

        var course = new CreateCourseRequest()
        {
            Title = "Test Course",
            TermId = term.TermId,
            InstructorId = Guid.NewGuid(),
            Status = CourseStatus.InProgress.ToString(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var result = await _courseService.CreateCourseAsync(course);
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("Instructor does not exist");
    }

    [Fact]
    public async Task CreateCourseAsync_ThrowsExceptionWhenTermDoesNotExistsAsync()
    {
        await ClearCourses();
        var (instructor, _) = await CreateInstructorAndTermAsync();

        var course = new CreateCourseRequest()
        {
            Title = "Test Course",
            TermId = Guid.NewGuid(),
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.InProgress.ToString(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var result = await _courseService.CreateCourseAsync(course);
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("Term does not exist");
    }

    [Fact]
    public async Task DeleteCourseAsync_DeletesCourseAsync()
    {
        await ClearCourses();
        var (instructor, term) = await CreateInstructorAndTermAsync();

        var course = new CreateCourseRequest()
        {
            Title = "Test Course",
            TermId = term.TermId,
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.InProgress.ToString(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var createdCourse = await _courseService.CreateCourseAsync(course);
        await _courseService.DeleteCourseAsync(createdCourse.Value.CourseId);


        var result = await _courseService.GetCourseAsync(createdCourse.Value.CourseId);
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("Course not found");
    }

    [Fact]
    public async Task GetAllCoursesAsync_ReturnsAllCoursesAsync()
    {
        await ClearCourses();
        var (instructor, term) = await CreateInstructorAndTermAsync();

        var course = new CreateCourseRequest()
        {
            Title = "Test Course",
            TermId = term.TermId,
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.InProgress.ToString(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var createdCourse = await _courseService.CreateCourseAsync(course);

        var course2 = new CreateCourseRequest()
        {
            Title = "Test Course2",
            TermId = term.TermId,
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.InProgress.ToString(),
            StartDate = DateTime.Now.AddDays(3),
            EndDate = DateTime.Now.AddDays(5)
        };

        var createdCourse2 =  await _courseService.CreateCourseAsync(course2);

        var courses = await _courseService.GetAllCoursesAsync();
        courses.Should().ContainEquivalentOf(createdCourse.Value);
        courses.Should().ContainEquivalentOf(createdCourse2.Value);
        courses.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateCourseAsync_UpdatesCourseAsync()
    {
        await ClearCourses();
        var (instructor, term) = await CreateInstructorAndTermAsync();

        var course = new CreateCourseRequest()
        {
            Title = "Test Course",
            TermId = term.TermId,
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.InProgress.ToString(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var createdCourse = await _courseService.CreateCourseAsync(course);
        var updateRequest = new UpdateCourseRequest()
        {
            CourseId = createdCourse.Value.CourseId,
            Title = "Updated Course",
            TermId = term.TermId,
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.Completed.ToString(),
            StartDate = DateTime.Now.AddDays(2),
            EndDate = DateTime.Now.AddDays(4)
        };

        var updatedTerm = await _courseService.UpdateCourseAsync(updateRequest);

        var courseFromDb = await _courseService.GetCourseAsync(createdCourse.Value.CourseId);
        courseFromDb.Should().BeEquivalentTo(updatedTerm);
        courseFromDb.Should().NotBeEquivalentTo(createdCourse);
        courseFromDb.Value.CourseId.Should().Be(createdCourse.Value.CourseId);
    }

    private async Task ClearCourses() => await _dbAccessAsync.GetConnection().DeleteAllAsync<Course>();

    private async Task<Tuple<Instructor, Term>> CreateInstructorAndTermAsync()
    {
        var instructor = new Instructor() { InstructorId = Guid.NewGuid() };
        await _dbAccessAsync.GetConnection().InsertAsync(instructor);

        var term = new Term() { TermId = Guid.NewGuid() };
        await _dbAccessAsync.GetConnection().InsertAsync(term);

        return new Tuple<Instructor, Term>(instructor, term);
    }
}
