using System.ComponentModel.DataAnnotations;
using WGU_App_RileyJuniewic.Data.Misc.Attributes;
using WGU_App_RileyJuniewic.Data.Models.Enums;

namespace WGU_App_RileyJuniewic.Data.Dtos.Course;

public class CreateCourseRequest : BindingModel
{
    protected Guid _termId;
    [Required]
    public Guid TermId
    {
        get => _termId;
        set
        {
            _termId = value;
            OnPropertyChanged(nameof(TermId));
            Validate(nameof(TermId), _termId);
        }
    }

    protected Guid _instructorId;
    public Guid InstructorId
    {
        get => _instructorId;
        set
        {
            _instructorId = value;
            OnPropertyChanged(nameof(InstructorId));
            Validate(nameof(InstructorId), _instructorId);
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


    protected CourseStatus _status;
    [Required]
    public CourseStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged(nameof(Status));
            Validate(nameof(Status), _status);
        }
    }

    protected DateTime _startDate;
    [Required]
    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;
            OnPropertyChanged(nameof(StartDate));
            Validate(nameof(StartDate), _startDate);
        }
    }

    protected DateTime _endDate;
    [Required]
    [DateTimeComparer(nameof(StartDate), true)]
    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            OnPropertyChanged(nameof(EndDate));
            Validate(nameof(EndDate), _endDate);
        }
    } 
    
    public CreateCourseRequest()
    {
        ValidateAll<CreateCourseRequest>();
    }
}