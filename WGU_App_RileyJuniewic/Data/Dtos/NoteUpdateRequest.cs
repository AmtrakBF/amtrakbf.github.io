using System.ComponentModel.DataAnnotations;

namespace WGU_App_RileyJuniewic.Data.Dtos.Note;

public class UpdateNoteRequest : CreateNoteRequest
{
    protected Guid _noteId;
    [Required]
    public Guid NoteId
    {
        get => _noteId;
        set
        {
            _noteId = value;
            OnPropertyChanged(nameof(NoteId));
            Validate(nameof(NoteId), _noteId);
        }
    }

    public UpdateNoteRequest()
    {
        ValidateAll<UpdateNoteRequest>();
    }
}