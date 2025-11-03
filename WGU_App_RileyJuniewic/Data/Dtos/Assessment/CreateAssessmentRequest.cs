using System.ComponentModel.DataAnnotations;
using WGU_App_RileyJuniewic.Data.Misc.Attributes;
using WGU_App_RileyJuniewic.Data.Models.Enums;

namespace WGU_App_RileyJuniewic.Data.Dtos.Assessment;

public class CreateAssessmentRequest : BindingModel
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

    protected string _name = "";
    [Required(AllowEmptyStrings = false)]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
            Validate(nameof(Name), _name);
        }
    }

    protected string _type = AssessmentType.Performance.ToString();
    [Required]
    public string Type
    {
        get => _type;
        set
        {
            _type = value;
            OnPropertyChanged(nameof(Type));
            Validate(nameof(Type), _type);
        }
    }

    protected DateTime _startDate = DateTime.Now;
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

    protected DateTime _endDate = DateTime.Now.AddMonths(1);
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

    public CreateAssessmentRequest()
    {
        ValidateAll<CreateAssessmentRequest>();
    }
}