using System.ComponentModel.DataAnnotations;

namespace WGU_App_RileyJuniewic.Data.Dtos;

public class UserCreateRequest : BindingModel
{
    private string _username = "";
    private string _password = "";
    private string _confirmPassword = "";

    [Required(AllowEmptyStrings = false)]
    public string Name
    {
        get => _username;
        set => SetValue(nameof(Name), ref _username, value);
    }

    [Required(AllowEmptyStrings = false)]
    public string Password
    {
        get => _password;
        set => SetValue(nameof(Password), ref _password, value);
    }

    [Required(AllowEmptyStrings = false)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetValue(nameof(ConfirmPassword), ref _confirmPassword, value);
    }

    public UserCreateRequest()
    {
        ValidateAll<UserCreateRequest>();
    }
}