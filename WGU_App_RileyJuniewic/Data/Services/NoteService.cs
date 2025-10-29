using WGU_App_RileyJuniewic.Data.Dtos.Note;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface INoteService
{
    Task<IEnumerable<Note>> GetAllNotesAsync();
    Task<Note> GetNoteAsync(Guid id);
    Task<Note> CreateNoteAsync(CreateNoteRequest request);
    Task<Note> UpdateNoteAsync(UpdateNoteRequest request);
    Task DeleteNoteAsync(Guid id);
}

public class NoteService(SqlDataAccessAsync sqlDataAccess) : INoteService
{
    public async Task<Note> CreateNoteAsync(CreateNoteRequest request)
    {
        var note = Note.CreateNewInstance(request.CourseId, request.Title, request.Content);
        await ValidateNoteAsync(note);
        await sqlDataAccess.GetConnection().InsertAsync(note);
        return note;
    }

    public async Task DeleteNoteAsync(Guid id) =>
        await sqlDataAccess.GetConnection().Table<Note>().Where(x => x.NoteId == id).DeleteAsync();

    public async Task<IEnumerable<Note>> GetAllNotesAsync() => await sqlDataAccess.GetConnection().Table<Note>().ToListAsync();

    public async Task<Note> GetNoteAsync(Guid id)
    {
        var note = await sqlDataAccess.GetConnection().Table<Note>().Where(x => x.NoteId == id).FirstOrDefaultAsync();
        if (note == null)
            throw new UserException("Note not found");
        return note;
    }

    public async Task<Note> UpdateNoteAsync(UpdateNoteRequest request)
    {
        var note = Note.CreateInstance(request.NoteId, request.CourseId, request.Title, request.Content);
        await ValidateNoteAsync(note);
        await sqlDataAccess.GetConnection().UpdateAsync(note);
        return note;
    }

    private async Task<bool> ValidateNoteAsync(Note note)
    {
        var existingCourse = await sqlDataAccess.GetConnection().Table<Course>().Where(x => x.CourseId == note.CourseId).FirstOrDefaultAsync();
        if (existingCourse == null)
            throw new UserException("Course not found");

        return true;
    }
}