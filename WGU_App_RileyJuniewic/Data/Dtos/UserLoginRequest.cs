using System.ComponentModel.DataAnnotations;

namespace WGU_App_RileyJuniewic.Data.Dtos;

public class UserLoginRequest : BindingModel
{
    private string _username = "";
    private string _password = "";

    [Required(AllowEmptyStrings = false)]
    public string Username
    {
        get => _username;
        set => SetValue(nameof(Username), ref _username, value);
    }

    [Required(AllowEmptyStrings = false)]
    public string Password
    {
        get => _password;
        set => SetValue(nameof(Password), ref _password, value);
    }

    public UserLoginRequest()
    {
        ValidateAll<UserLoginRequest>();
    }
}