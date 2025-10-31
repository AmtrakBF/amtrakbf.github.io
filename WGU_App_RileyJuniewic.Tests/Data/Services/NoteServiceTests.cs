using Ardalis.Result;
using FluentAssertions;
using WGU_App_RileyJuniewic.Data.Dtos.Note;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;
using WGU_App_RileyJuniewic.Data.Services;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Microsoft.DependencyInjection.Attributes;

namespace WGU_App_RileyJuniewic.Tests.Data.Services;

public class NoteServiceTests : TestBedWithDI<TestServiceProvider>
{
    [Inject] protected INoteService _noteService { get; set; } = null!;
    [Inject] protected SqlDataAccessAsync _dbAccessAsync { get; set; } = null!;

    public NoteServiceTests(ITestOutputHelper testOutputHelper, TestServiceProvider fixture) : base(testOutputHelper, fixture)
    {
    }

    [Fact]
    public async Task CreateNoteAsync_CreatesDBEntryAsync()
    {
        await ClearNotes();

        var course = new Course() { CourseId = Guid.NewGuid() };
        await _dbAccessAsync.GetConnection().InsertAsync(course);

        var note = new CreateNoteRequest()
        {
            CourseId = course.CourseId,
            Title = "Test Note",
            Content = "Test Content"
        };

        var result = await _noteService.CreateNoteAsync(note);
        var dbNote = await _dbAccessAsync.GetConnection().Table<Note>().Where(x => x.NoteId == result.Value.NoteId).FirstOrDefaultAsync();
        result.Value.Should().BeEquivalentTo(dbNote);
    }

    [Fact]
    public async Task CreateNoteAsync_ThrowsExceptionWhenCourseNotFoundAsync()
    {
        await ClearNotes();

        var note = new CreateNoteRequest()
        {
            CourseId = Guid.NewGuid(),
            Title = "Test Note",
            Content = "Test Content"
        };

        var result = await _noteService.CreateNoteAsync(note);
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("Course not found");
    }

    [Fact]
    public async Task DeleteNoteAsync_DeletesDBEntryAsync()
    {
        await ClearNotes();

        var course = new Course() { CourseId = Guid.NewGuid() };
        await _dbAccessAsync.GetConnection().InsertAsync(course);

        var note = new CreateNoteRequest()
        {
            CourseId = course.CourseId,
            Title = "Test Note",
            Content = "Test Content"
        };

        var result = await _noteService.CreateNoteAsync(note);
        await _noteService.DeleteNoteAsync(result.Value.NoteId);
        var dbNote = await _dbAccessAsync.GetConnection().Table<Note>().Where(x => x.NoteId == result.Value.NoteId).FirstOrDefaultAsync();
        dbNote.Should().BeNull();
    }

    [Fact]
    public async Task GetAllNotesAsync_ReturnsAllNotesAsync()
    {
        await ClearNotes();

        var course = new Course() { CourseId = Guid.NewGuid() };
        await _dbAccessAsync.GetConnection().InsertAsync(course);

        var note = new CreateNoteRequest()
        {
            CourseId = course.CourseId,
            Title = "Test Note",
            Content = "Test Content"
        };

        var note2 = new CreateNoteRequest()
        {
            CourseId = course.CourseId,
            Title = "Test Note 2",
            Content = "Test Content 2"
        };

        var result = await _noteService.CreateNoteAsync(note);
        var result2 = await _noteService.CreateNoteAsync(note2);
        var notes = await _noteService.GetAllNotesAsync();
        notes.Should().ContainEquivalentOf(result.Value);
        notes.Should().ContainEquivalentOf(result2.Value);
    }
    
    [Fact]
    public async Task GetNoteAsync_ReturnsNoteAsync()
    {
        await ClearNotes();

        var course = new Course() { CourseId = Guid.NewGuid() };
        await _dbAccessAsync.GetConnection().InsertAsync(course);

        var note = new CreateNoteRequest()
        {
            CourseId = course.CourseId,
            Title = "Test Note",
            Content = "Test Content"
        };

        var result = await _noteService.CreateNoteAsync(note);
        var dbNote = await _noteService.GetNoteAsync(result.Value.NoteId);
        dbNote.Should().BeEquivalentTo(result);
    }

    [Fact]
    public async Task GetNoteAsync_ThrowsExceptionWhenNoteNotFoundAsync()
    {
        await ClearNotes();

        var result = await _noteService.GetNoteAsync(Guid.NewGuid());
        result.IsError().Should().BeTrue();
        result.Errors.First().Should().Be("Note not found");
    }

    [Fact]
    public async Task UpdateNoteAsync_UpdatesDBEntryAsync()
    {
        await ClearNotes();

        var course = new Course() { CourseId = Guid.NewGuid() };
        await _dbAccessAsync.GetConnection().InsertAsync(course);

        var note = new CreateNoteRequest()
        {
            CourseId = course.CourseId,
            Title = "Test Note",
            Content = "Test Content"
        };

        var result = await _noteService.CreateNoteAsync(note);
        var updateNote = new UpdateNoteRequest()
        {
            NoteId = result.Value.NoteId,
            CourseId = course.CourseId,
            Title = "Test Note 2",
            Content = "Test Content 2"
        };
        await _noteService.UpdateNoteAsync(updateNote);
        var dbNote = await _dbAccessAsync.GetConnection().Table<Note>().Where(x => x.NoteId == result.Value.NoteId).FirstOrDefaultAsync();
        dbNote.NoteId.Should().Be(result.Value.NoteId);
        dbNote.CourseId.Should().Be(course.CourseId);
        dbNote.Title.Should().Be(updateNote.Title);
        dbNote.Content.Should().Be(updateNote.Content);
    }

    private async Task ClearNotes() => await _dbAccessAsync.GetConnection().DeleteAllAsync<Note>();
}
