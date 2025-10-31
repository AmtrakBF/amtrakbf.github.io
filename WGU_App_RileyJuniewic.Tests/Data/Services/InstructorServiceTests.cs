using Ardalis.Result;
using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Dtos.Instructor;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;
using WGU_App_RileyJuniewic.Data.Services;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Microsoft.DependencyInjection.Attributes;

namespace WGU_App_RileyJuniewic.Tests.Data.Services;

public class InstructorServiceTests : TestBedWithDI<TestServiceProvider>
{
    [Inject] protected IInstructorService _instructorService { get; set; } = null!;
    [Inject] protected SqlDataAccessAsync _dbAccessAsync { get; set; } = null!;

    public InstructorServiceTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {

    }

    [Fact]
    public async Task CreateInstructorAsync_CreatesDBEntryAsync()
    {
        await ClearInstructors();

        var instructor = new CreateInstructorRequest()
        {
            Name = "Bob Smith",
            Email = "bob.smith@example.com",
            Phone = "1234567890"
        };

        var result = await _instructorService.CreateInstructorAsync(instructor);
        var dbInstructor = await _dbAccessAsync.GetConnection().Table<Instructor>().Where(x => x.InstructorId == result.Value.InstructorId).FirstOrDefaultAsync();
        result.Value.Should().BeEquivalentTo(dbInstructor);
    }

    [Fact]
    public async Task CreateInstructorAsync_ThrowsExceptionWhenEmailAlreadyExistsAsync()
    {
        await ClearInstructors();

        var instructor = new CreateInstructorRequest()
        {
            Name = "Bob Smith",
            Email = "bob.smith@example.com",
            Phone = "1234567890"
        };

        await _instructorService.CreateInstructorAsync(instructor);

        var instructor2 = new CreateInstructorRequest()
        {
            Name = "Bob Smith 2",
            Email = "BOB.SMITH@example.com",
            Phone = "54254535"
        };

        var result = await _instructorService.CreateInstructorAsync(instructor2);
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("Instructor with email already exists");
    }

    [Fact]
    public async Task CreateInstructorAsync_ThrowsExceptionWhenPhoneNumberAlreadyExistsAsync()
    {
        await ClearInstructors();

        var instructor = new CreateInstructorRequest()
        {
            Name = "Bob Smith",
            Email = "bob.smith@example.com",
            Phone = "1234567890"
        };

        await _instructorService.CreateInstructorAsync(instructor);
        
        var instructor2 = new CreateInstructorRequest()
        {
            Name = "Bob Smith 2",
            Email = "bob.smith2@example.com",
            Phone = "1234567890"
        };

        var result = await _instructorService.CreateInstructorAsync(instructor2);
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("Instructor with phone number already exists");
    }


    [Fact]
    public async Task DeleteInstructorAsync()
    {
        await ClearInstructors();

        var instructor = new Instructor() { InstructorId = Guid.NewGuid() };
        await _dbAccessAsync.GetConnection().InsertAsync(instructor);

        await _instructorService.DeleteInstructorAsync(instructor.InstructorId);

        var dbInstructor = await _dbAccessAsync.GetConnection().Table<Instructor>().Where(x => x.InstructorId == instructor.InstructorId).FirstOrDefaultAsync();
        dbInstructor.Should().BeNull();
    }

    [Fact]
    public async Task GetAllInstructorsAsync_ReturnsAllInstructorsAsync()
    {
        await ClearInstructors();

        var instructor = new Instructor() { InstructorId = Guid.NewGuid() };
        await _dbAccessAsync.GetConnection().InsertAsync(instructor);

        var instructor2 = new Instructor() { InstructorId = Guid.NewGuid() };
        await _dbAccessAsync.GetConnection().InsertAsync(instructor2);

        var instructors = await _instructorService.GetAllInstructorsAsync();
        instructors.Should().ContainEquivalentOf(instructor);
        instructors.Should().ContainEquivalentOf(instructor2);
    }

    [Fact]
    public async Task GetInstructorAsync_ReturnsInstructorAsync()
    {
        await ClearInstructors();

        var instructor = new Instructor() { InstructorId = Guid.NewGuid() };
        await _dbAccessAsync.GetConnection().InsertAsync(instructor);

        var result = await _instructorService.GetInstructorAsync(instructor.InstructorId);
        result.Value.Should().BeEquivalentTo(instructor);
    }

    [Fact]
    public async Task GetInstructorAsync_ThrowsExceptionWhenInstructorNotFoundAsync()
    {
        await ClearInstructors();

        var result = await _instructorService.GetInstructorAsync(Guid.NewGuid());
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("Instructor not found");
    }

    [Fact]
    public async Task UpdateInstructorAsync_UpdatesInstructorAsync()
    {
        await ClearInstructors();

        var instructor = new Instructor() { InstructorId = Guid.NewGuid(), Name = "Bob Smith", Email = "bob.smith@example.com", Phone = "1234567890" };
        await _dbAccessAsync.GetConnection().InsertAsync(instructor);

        var updateRequest = new UpdateInstructorRequest()
        {
            InstructorId = instructor.InstructorId,
            Name = "Bob Smith 2",
            Email = "bob.smith@example.com",
            Phone = "54254535"
        };

        var result = await _instructorService.UpdateInstructorAsync(updateRequest);
        var instructorResult = result.Value;
        instructorResult.InstructorId.Should().Be(updateRequest.InstructorId);
        instructorResult.Name.Should().Be(updateRequest.Name);
        instructorResult.Email.Should().Be(updateRequest.Email);
        instructorResult.Phone.Should().Be(updateRequest.Phone);
    }

    private async Task ClearInstructors() => await _dbAccessAsync.GetConnection().DeleteAllAsync<Instructor>();

}
