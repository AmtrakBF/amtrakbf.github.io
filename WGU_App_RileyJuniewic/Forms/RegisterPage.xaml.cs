using WGU_App_RileyJuniewic.Data.ViewModels;

namespace WGU_App_RileyJuniewic.Forms;

public sealed partial class RegisterPage : ContentPage
{
    private readonly RegisterViewModel _registerViewModel;

    public RegisterPage()
    {
        _registerViewModel = ServiceHelper.GetService<RegisterViewModel>();
        BindingContext = _registerViewModel;

        _registerViewModel.RegisterEvent += (sender, args) =>
        {
            Shell.Current.IsVisible = false;
            Shell.Current.GoToAsync("//CurrentTermPage", true);
        };

        InitializeComponent();
    }
    
}