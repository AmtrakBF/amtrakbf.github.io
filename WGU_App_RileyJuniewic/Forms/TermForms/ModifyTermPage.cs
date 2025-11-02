using WGU_App_RileyJuniewic.Data.ViewModels;

namespace WGU_App_RileyJuniewic.Forms;

public partial class ModifyTermPage : ContentPage
{
    private readonly CurrentTermViewModel _viewModel;

    public ModifyTermPage(CurrentTermViewModel viewModel)
	{
		BindingContext = viewModel;
        _viewModel = viewModel;
		InitializeComponent();
    }
}