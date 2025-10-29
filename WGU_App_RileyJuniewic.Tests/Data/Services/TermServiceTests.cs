using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Dtos.Term;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;
using WGU_App_RileyJuniewic.Data.Services;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Microsoft.DependencyInjection.Attributes;

namespace WGU_App_RileyJuniewic.Tests.Data.Services;

public class TermServiceTests : TestBedWithDI<TestServiceProvider>
{
    [Inject] protected ITermService _termService { get; set; } = null!;
    [Inject] protected SqlDataAccessAsync _dbAccessAsync { get; set; } = null!;

    public TermServiceTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {

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
        var termFromDb = await _dbAccessAsync.GetConnection().GetAsync<Term>(createdTerm.TermId);

        termFromDb.Should().BeEquivalentTo(createdTerm);
    }

    [Fact]
    public async Task CreateTermAsync_ThrowsExceptionWhenTermOverlapsAsync()
    {
        await ClearTerms();

        var term = new CreateTermRequest()
        {
            Title = "Test Term",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 2, 2)
        };

        await _termService.CreateTermAsync(term);

        var term2 = new CreateTermRequest()
        {
            Title = "Test Term2",
            StartDate = new DateTime(2023, 2, 2),
            EndDate = new DateTime(2023, 3, 3)
        };

        var exception = await Assert.ThrowsAsync<UserException>(() => _termService.CreateTermAsync(term2));
        exception.Message.Should().Be("Term overlaps with an existing term");
    }

    [Fact]
    public async Task CreateTermAsync_ThrowsExceptionWhenTermTitleExistsAsync()
    {
        await ClearTerms();

        var term = new CreateTermRequest()
        {
            Title = "Test Term",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 2, 2)
        };

        await _termService.CreateTermAsync(term);

        var term2 = new CreateTermRequest()
        {
            Title = "Test Term",
            StartDate = new DateTime(2023, 4, 4),
            EndDate = new DateTime(2023, 5, 5)
        };

        var exception = await Assert.ThrowsAsync<UserException>(() => _termService.CreateTermAsync(term2));
        exception.Message.Should().Be("Term with the same title already exists");
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

        await _termService.DeleteTermAsync(createdTerm.TermId);
        var termFromDb = await _dbAccessAsync.GetConnection().Table<Term>().Where(x => x.TermId == createdTerm.TermId).FirstOrDefaultAsync();
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

        var updatedTerm = new Term()
        {
            TermId = createdTerm.TermId,
            Title = "Updated Term",
            StartDate = new DateTime(2023, 3, 3),
            EndDate = new DateTime(2023, 4, 4)
        };
        
        await _termService.UpdateTermAsync(updatedTerm);
        var termFromDb = await _dbAccessAsync.GetConnection().GetAsync<Term>(updatedTerm.TermId);
        termFromDb.Should().BeEquivalentTo(updatedTerm);
    }

    private async Task ClearTerms() => await _dbAccessAsync.GetConnection().DeleteAllAsync<Term>();
}
