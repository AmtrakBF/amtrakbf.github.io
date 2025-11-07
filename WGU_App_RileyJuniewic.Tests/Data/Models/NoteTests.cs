
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
        var note = Note.CreateNewInstance(Guid.NewGuid(), "Test Note", "This is a test note.");

        await _dbAccessAsync.GetConnectionAsync().InsertAsync(note);
        var noteFromDb = await _dbAccessAsync.GetConnectionAsync().GetAsync<Note>(note.NoteId);
        noteFromDb.Should().BeEquivalentTo(note);
    }
}
