using WGU_App_RileyJuniewic.Data.ViewModels;

namespace WGU_App_RileyJuniewic.Forms;

public partial class AddTermPage : ContentPage
{
    private readonly CurrentTermViewModel _viewModel;

    public AddTermPage(CurrentTermViewModel viewModel)
	{
		BindingContext = viewModel;
        _viewModel = viewModel;
		InitializeComponent();
    }
}