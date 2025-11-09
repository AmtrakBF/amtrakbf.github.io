using Ardalis.Result;
using FluentAssertions;
using WGU_App_RileyJuniewic.Data;
using WGU_App_RileyJuniewic.Data.Dtos.Term;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;
using WGU_App_RileyJuniewic.Data.Services;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Microsoft.DependencyInjection.Attributes;

namespace WGU_App_RileyJuniewic.Tests.Data.Services;

public class TermServiceTests : TestBedWithDI<TestServiceProvider>
{
    [Inject] protected ITermService _termService { get; set; } = null!;
    [Inject] protected UserStore _userStore { get; set; } = null!;
    [Inject] protected SqlDataAccessAsync _dbAccessAsync { get; set; } = null!;

    public TermServiceTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {
        _userStore.SetUser(new User { UserId = Guid.NewGuid() });
    }

    [Fact]
    public async Task CreateTermAsync_CreatesDBEntryAsync()
    {
        await ClearTerms();

        var term = new CreateTermRequest()
        {
            Title = "Test Term",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        var createdTerm = await _termService.CreateTermAsync(term);
        var connection = await _dbAccessAsync.GetConnectionAsync();
        var termFromDb = await connection.GetAsync<Term>(createdTerm.Value.TermId);

        termFromDb.Should().BeEquivalentTo(createdTerm.Value);
    }


    [Fact]
    public async Task DeleteTermAsync_DeletesTermAsync()
    {
        await ClearTerms();

        var term = new CreateTermRequest()
        {
            Title = "Test Term",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 2, 2)
        };

        var createdTerm = await _termService.CreateTermAsync(term);

        await _termService.DeleteTermAsync(createdTerm.Value.TermId);
        var connection = await _dbAccessAsync.GetConnectionAsync();
        var termFromDb = await connection.Table<Term>().Where(x => x.TermId == createdTerm.Value.TermId).FirstOrDefaultAsync();
        termFromDb.Should().BeNull();
    }

    [Fact]
    public async Task UpdateTermAsync_UpdatesTermAsync()
    {
        await ClearTerms();

        var term = new CreateTermRequest()
        {
            Title = "Test Term",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 2, 2)
        };

        var createdTerm = await _termService.CreateTermAsync(term);

        var updateRequest = new UpdateTermRequest()
        {
            Id = createdTerm.Value.TermId,
            Title = "Updated Term",
            StartDate = new DateTime(2023, 3, 3),
            EndDate = new DateTime(2023, 4, 4)
        };

        var updatedTerm = await _termService.UpdateTermAsync(updateRequest);
        var connection = await _dbAccessAsync.GetConnectionAsync();
        var termFromDb = await connection.GetAsync<Term>(createdTerm.Value.TermId);
        termFromDb.Should().BeEquivalentTo(updatedTerm.Value);
        termFromDb.Should().NotBeEquivalentTo(createdTerm.Value);
        termFromDb.TermId.Should().Be(createdTerm.Value.TermId);
    }

    [Fact]
    public async Task GetAllTermsAsync_ReturnsAllTermsAsync()
    {
        await ClearTerms();

        var term = new CreateTermRequest()
        {
            Title = "Test Term",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 2, 2)
        };

        var createdTerm = await _termService.CreateTermAsync(term);

        var term2 = new CreateTermRequest()
        {
            Title = "Test Term2",
            StartDate = new DateTime(2023, 3, 3),
            EndDate = new DateTime(2023, 4, 4)
        };

        var createdTerm2 = await _termService.CreateTermAsync(term2);

        var terms = await _termService.GetAllTermsAsync();
        terms.IsError().Should().BeFalse();
        terms.Value.Should().ContainEquivalentOf(createdTerm.Value);
        terms.Value.Should().ContainEquivalentOf(createdTerm2.Value);
    }

    [Theory]
    [InlineData("358aa2d3-69e3-4123-8deb-d71ef308502a", "Test Term", "2023-07-01", "2023-08-01", "Term with the same title already exists")]
    [InlineData("358aa2d3-69e3-4123-8deb-d71ef308501a", "Test Term", "2023-07-01", "2023-08-01")]
    [InlineData("358aa2d3-69e3-4123-8deb-d71ef308502a", "Test Term2", "2023-05-01", "2023-08-01", "Term overlaps with an existing term")]
    [InlineData("358aa2d3-69e3-4123-8deb-d71ef308502a", "Test Term2", "2022-05-01", "2022-12-01", "Term must be less than 6 months")]
    [InlineData("358aa2d3-69e3-4123-8deb-d71ef308501a", "Test Term2", "2023-05-01", "2023-08-01")]
    public async Task ValidateTermAsync_ValidatesTerm(Guid termId, string title, DateTime startDate, DateTime endDate, string? message = null)
    {
        await ClearTerms();

        var user = _userStore.GetUser();
        if (user.IsError())
            throw new Exception("User is not logged in");

        var termBase = Term.CreateInstance(new Guid("358aa2d3-69e3-4123-8deb-d71ef308501a"), user.Value.UserId, "Test Term", new DateTime(2023, 5, 1), new DateTime(2023, 6, 1));
            var connection = await _dbAccessAsync.GetConnectionAsync();
        await connection.InsertAsync(termBase);

        var term = Term.CreateInstance(termId, user.Value.UserId, title, startDate, endDate);
        var termService = new TermService(_dbAccessAsync, _userStore);

        if (message != null)
        {
            var result = await termService.ValidateTermAsync(term);
            result.IsError().Should().BeTrue();
            result.Errors.First().Should().Be(message);
        } else
        {
            var result = await termService.ValidateTermAsync(term);
            result.IsError().Should().BeFalse();
        }
    }

    private async Task ClearTerms()
    {
        var connection = await _dbAccessAsync.GetConnectionAsync();
        await connection.DeleteAllAsync<Term>();
    }
}
