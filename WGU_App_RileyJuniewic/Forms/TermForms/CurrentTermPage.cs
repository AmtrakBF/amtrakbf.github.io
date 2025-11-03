using WGU_App_RileyJuniewic.Data.ViewModels;
using WGU_App_RileyJuniewic.Forms.CourseForms;
using WGU_App_RileyJuniewic.Forms.TermForms;

namespace WGU_App_RileyJuniewic.Forms;

public partial class CurrentTermPage : ContentPage
{
    private readonly CurrentTermViewModel _viewModel;

    public CurrentTermPage(CurrentTermViewModel viewModel)
	{
		BindingContext = viewModel;
        _viewModel = viewModel;
		InitializeComponent();
    }

    private void Add_Course_Clicked(object sender, EventArgs e)
	{
		if (_viewModel.Term is null) return;

		var navigationParameter = new Dictionary<string, object>
		{
			{ "Term", _viewModel.Term }
		};
        _ = Shell.Current.GoToAsync(nameof(AddCoursePage), navigationParameter);
    }

    private void Add_Term_Clicked(object sender, EventArgs e)
	{
        _ = Shell.Current.GoToAsync(nameof(AddTermPage));
    }
}