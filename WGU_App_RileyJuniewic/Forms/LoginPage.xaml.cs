using WGU_App_RileyJuniewic.Data.ViewModels;

namespace WGU_App_RileyJuniewic.Forms;

public sealed partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _loginViewModel;
    private readonly RegisterViewModel _registerViewModel;

    public LoginPage()
    {
        _loginViewModel = ServiceHelper.GetService<LoginViewModel>();
        _registerViewModel = ServiceHelper.GetService<RegisterViewModel>();
        BindingContext = _loginViewModel;

        _loginViewModel.LoginEvent += (sender, args) =>
        {
            Shell.Current.IsVisible = false;
            Shell.Current.GoToAsync("//CurrentTermPage", true);
        };

        InitializeComponent();
    }

    private void RegisterEventHandler(object sender, EventArgs e)
    {
        Shell.Current.IsVisible = false;
        Shell.Current.GoToAsync(nameof(RegisterPage), true);
    }
}