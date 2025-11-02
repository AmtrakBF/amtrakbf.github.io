using WGU_App_RileyJuniewic.Data.ViewModels;
using WGU_App_RileyJuniewic.Forms.CourseForms;

namespace WGU_App_RileyJuniewic.Forms;

public partial class TermPage : ContentPage
{
    private readonly CurrentTermViewModel _viewModel;

    public TermPage(CurrentTermViewModel viewModel)
	{
		BindingContext = viewModel;
        _viewModel = viewModel;
		InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
	{
		if (_viewModel.Term is null) return;

		var navigationParameter = new Dictionary<string, object>
		{
			{ "Term", _viewModel.Term }
		};
        _ = Shell.Current.GoToAsync(nameof(AddCoursePage), navigationParameter);
    }
}