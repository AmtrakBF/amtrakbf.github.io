using WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;

namespace WGU_App_RileyJuniewic.Forms;

public partial class ViewTermsPage : ContentPage
{
    private readonly ViewTermsViewModel _viewModel;

    public ViewTermsPage(ViewTermsViewModel viewModel)
	{
		BindingContext = viewModel;
        _viewModel = viewModel;
		InitializeComponent();
    }

    private void Add_Term_Clicked(object sender, EventArgs e)
    {
        _ = Shell.Current.GoToAsync(nameof(AddTermPage));
    }
}