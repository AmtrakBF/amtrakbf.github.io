
using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Tests.Data.Models;

public class NoteTests : BaseTest
{
    public NoteTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {
    }

    [Fact]
    public async Task CreateNote_CreatesDBEntryAsync()
    {
        var note = Note.CreateNewInstance("Test Note", "This is a test note.");

        await _dbAccessAsync.GetConnection().InsertAsync(note);
        var noteFromDb = await _dbAccessAsync.GetConnection().GetAsync<Note>(note.NoteId);
        noteFromDb.Should().BeEquivalentTo(note);
    }
}
