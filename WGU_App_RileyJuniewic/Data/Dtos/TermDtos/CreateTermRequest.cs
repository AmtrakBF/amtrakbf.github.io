using System.ComponentModel.DataAnnotations;
using WGU_App_RileyJuniewic.Data.Misc.Attributes;

namespace WGU_App_RileyJuniewic.Data.Dtos.Term;

public class CreateTermRequest : BindingModel
{
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
            Validate(nameof(EndDate), _endDate);
        }
    }

    protected DateTime _endDate = DateTime.Now.AddDays(1);
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
            Validate(nameof(StartDate), _startDate);
        }
    }

    public CreateTermRequest()
    {
        ValidateAll<CreateTermRequest>();
    }
}