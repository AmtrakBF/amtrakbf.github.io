using System.ComponentModel.DataAnnotations;

namespace WGU_App_RileyJuniewic.Data.Dtos.Instructor;

public class CreateInstructorRequest : BindingModel
{
    protected string _name = "";
    [Required(AllowEmptyStrings = false)]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
            Validate(nameof(Name), value);
        }
    }

    protected string _email = "";
    [Required(AllowEmptyStrings = false)]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
            Validate(nameof(Email), value);
        }
    }

    protected string _phone = "";
    [Required(AllowEmptyStrings = false)]
    [MaxLength(20, ErrorMessage = "Phone Number cannot be more than 20 digits")]
    [MinLength(10, ErrorMessage = "Phone Number must be at least 10 digits")]
    [RegularExpression(@"^[0-9-]*$", ErrorMessage = "Phone number must only contain numbers and dashes.")]
    public string Phone
    {
        get => _phone;
        set
        {
            _phone = value;
            OnPropertyChanged(nameof(Phone));
            Validate(nameof(Phone), value);
        }
    }

    public CreateInstructorRequest()
    {
        ValidateAll<CreateInstructorRequest>();
    }
}