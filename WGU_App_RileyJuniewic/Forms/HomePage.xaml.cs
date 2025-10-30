using WGU_App_RileyJuniewic.Data.ViewModels;

namespace WGU_App_RileyJuniewic.Forms;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
	{
        _ = Shell.Current.GoToAsync("AddCoursePage");
    }
}