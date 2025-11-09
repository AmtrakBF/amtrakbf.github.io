
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
        var term = Term.CreateNewInstance("Test Term", Guid.NewGuid(), DateTime.Now, DateTime.Now.AddDays(30));
        
        var connection = await _dbAccessAsync.GetConnectionAsync();
        await connection.InsertAsync(term);
        var termFromDb = await connection.GetAsync<Term>(term.TermId);
        termFromDb.Should().BeEquivalentTo(term);
    }
}
