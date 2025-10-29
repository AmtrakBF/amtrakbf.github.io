using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
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
            Status = CourseStatus.InProgress,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var createdCourse = await _courseService.CreateCourseAsync(course);
        var courseFromDb = await _courseService.GetCourseAsync(createdCourse.CourseId);

        courseFromDb.Should().BeEquivalentTo(createdCourse);
    }

    [Fact]
    public async Task CreateCourseAsync_ThrowsExceptionWhenCourseOverlapsAsync()
    {
        await ClearCourses();

        var (instructor, term) = await CreateInstructorAndTermAsync();

        var course = new CreateCourseRequest()
        {
            Title = "Test Course",
            TermId = term.TermId,
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.InProgress,
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 2, 2)
        };

        await _courseService.CreateCourseAsync(course);

        var course2 = new CreateCourseRequest()
        {
            Title = "Test Course2",
            TermId = term.TermId,
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.InProgress,
            StartDate = new DateTime(2023, 2, 2),
            EndDate = new DateTime(2023, 3, 3)
        };
            
        var exception = await Assert.ThrowsAsync<UserException>(() => _courseService.CreateCourseAsync(course2));
        exception.Message.Should().Be("Course overlaps with an existing course");
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
            Status = CourseStatus.InProgress,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var exception = await Assert.ThrowsAsync<UserException>(() => _courseService.CreateCourseAsync(course));
        exception.Message.Should().Be("Instructor does not exist");
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
            Status = CourseStatus.InProgress,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var exception = await Assert.ThrowsAsync<UserException>(() => _courseService.CreateCourseAsync(course));
        exception.Message.Should().Be("Term does not exist");
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
            Status = CourseStatus.InProgress,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var createdCourse = await _courseService.CreateCourseAsync(course);
        await _courseService.DeleteCourseAsync(createdCourse.CourseId);

        var exception = await Assert.ThrowsAsync<UserException>(() => _courseService.GetCourseAsync(createdCourse.CourseId));
        exception.Message.Should().Be("Course not found");
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
            Status = CourseStatus.InProgress,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var createdCourse = await _courseService.CreateCourseAsync(course);

        var course2 = new CreateCourseRequest()
        {
            Title = "Test Course2",
            TermId = term.TermId,
            InstructorId = instructor.InstructorId,
            Status = CourseStatus.InProgress,
            StartDate = DateTime.Now.AddDays(3),
            EndDate = DateTime.Now.AddDays(5)
        };

        var createdCourse2 =  await _courseService.CreateCourseAsync(course2);

        var courses = await _courseService.GetAllCoursesAsync();
        courses.Should().ContainEquivalentOf(createdCourse);
        courses.Should().ContainEquivalentOf(createdCourse2);
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
            Status = CourseStatus.InProgress,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var createdCourse = await _courseService.CreateCourseAsync(course);
        createdCourse.Title = "Updated Course";
        await _courseService.UpdateCourseAsync(createdCourse);

        var courseFromDb = await _courseService.GetCourseAsync(createdCourse.CourseId);
        courseFromDb.Should().BeEquivalentTo(createdCourse);
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
