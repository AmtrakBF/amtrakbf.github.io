
using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Tests.Data.Models;

public class TermTests : BaseTest
{
    public TermTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {
    }

    [Fact]
    public async Task CreateTerm_CreatesDBEntryAsync()
    {
        var term = Term.CreateNewInstance("Test Term", DateTime.Now, DateTime.Now.AddDays(30));
        
        await _dbAccessAsync.GetConnection().InsertAsync(term);
        var termFromDb = await _dbAccessAsync.GetConnection().GetAsync<Term>(term.TermId);
        termFromDb.Should().BeEquivalentTo(term);
    }
}
