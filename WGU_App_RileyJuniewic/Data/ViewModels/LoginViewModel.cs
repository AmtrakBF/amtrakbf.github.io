using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels;

public class LoginViewModel : BindingModel
{
    private readonly IUserService _userService;
    private readonly UserStore _userStore;
    private UserLoginRequest _loginRequest = new();

    public UserLoginRequest LoginRequest
    {
        get => _loginRequest;
        set => SetValue(nameof(LoginRequest), ref _loginRequest, value);
    }

    public OnClickCommandAsync LoginCommandAsync { get; set; }
    public event EventHandler? LoginEvent;

    public LoginViewModel(IUserService userService, UserStore userStore)
    {
        _userService = userService;
        _userStore = userStore;

        LoginCommandAsync = new OnClickCommandAsync(LoginAsync, (obj) => !LoginRequest.HasErrors);
        LoginRequest.PropertyChanged += (sender, args) => LoginCommandAsync.RaiseCanExecuteChanged();
    }

    public async Task LoginAsync()
    {
        if (LoginRequest.HasErrors)
        {
            new ToastNotification(LoginRequest.GetAllErrors());
            return;
        }

        var user = await _userService.LoginAsync(LoginRequest);
        if (user.IsError())
        {
            new ToastNotification(user.Errors);
            return;
        }

        _userStore.SetUser(user.Value);
        LoginEvent?.Invoke(this, EventArgs.Empty);
    }
}