using WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;

namespace WGU_App_RileyJuniewic.Forms;

public partial class TermPage : ContentPage
{
    private readonly ViewTermsViewModel _viewModel;

    public TermPage(ViewTermsViewModel viewModel)
	{
		BindingContext = viewModel;
        _viewModel = viewModel;
		InitializeComponent();
    }
}