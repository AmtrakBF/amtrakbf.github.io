using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos.Note;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface INoteService
{
    Task<IEnumerable<Note>> GetAllNotesAsync();
    Task<Result<Note>> GetNoteAsync(Guid id);
    Task<Result<Note>> CreateNoteAsync(CreateNoteRequest request);
    Task<Result<Note>> UpdateNoteAsync(UpdateNoteRequest request);
    Task DeleteNoteAsync(Guid id);
}

public class NoteService(SqlDataAccessAsync sqlDataAccess) : INoteService
{
    public async Task<Result<Note>> CreateNoteAsync(CreateNoteRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetAllErrors());

        var note = Note.CreateNewInstance(request.CourseId, request.Title, request.Content);
        var result = await ValidateNoteAsync(note);
        if (result.IsError())
            return result;

        await sqlDataAccess.GetConnection().InsertAsync(note);
        return note;
    }

    public async Task DeleteNoteAsync(Guid id) =>
        await sqlDataAccess.GetConnection().Table<Note>().Where(x => x.NoteId == id).DeleteAsync();

    public async Task<IEnumerable<Note>> GetAllNotesAsync() => await sqlDataAccess.GetConnection().Table<Note>().ToListAsync();

    public async Task<Result<Note>> GetNoteAsync(Guid id)
    {
        var note = await sqlDataAccess.GetConnection().Table<Note>().Where(x => x.NoteId == id).FirstOrDefaultAsync();
        if (note == null)
            return Result.Error("Note not found");

        return note;
    }

    public async Task<Result<Note>> UpdateNoteAsync(UpdateNoteRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetAllErrors());
            
        var note = Note.CreateInstance(request.NoteId, request.CourseId, request.Title, request.Content);
        var result = await ValidateNoteAsync(note);
        if (result.IsError())
            return result;
            
        await sqlDataAccess.GetConnection().UpdateAsync(note);
        return note;
    }

    private async Task<Result> ValidateNoteAsync(Note note)
    {
        var existingCourse = await sqlDataAccess.GetConnection().Table<Course>().Where(x => x.CourseId == note.CourseId).FirstOrDefaultAsync();
        if (existingCourse == null)
            return Result.Error("Course not found");

        return Result.Success();
    }
}