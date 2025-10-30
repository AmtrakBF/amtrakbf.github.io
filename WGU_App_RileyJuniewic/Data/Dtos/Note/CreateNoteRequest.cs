using System.ComponentModel.DataAnnotations;

namespace WGU_App_RileyJuniewic.Data.Dtos.Note;

public class CreateNoteRequest : BindingModel
{
    protected Guid _courseId;
    [Required]
    public Guid CourseId
    {
        get => _courseId;
        set
        {
            _courseId = value;
            OnPropertyChanged(nameof(CourseId));
            Validate(nameof(CourseId), _courseId);
        }
    }

    protected string _title = "";
    [Required(AllowEmptyStrings = false)]
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
            Validate(nameof(Title), _title);
        }
    }

    protected string _content = "";
    [Required(AllowEmptyStrings = false)]
    public string Content
    {
        get => _content;
        set
        {
            _content = value;
            OnPropertyChanged(nameof(Content));
            Validate(nameof(Content), _content);
        }
    }

    public CreateNoteRequest()
    {
        ValidateAll<CreateNoteRequest>();
    }
}