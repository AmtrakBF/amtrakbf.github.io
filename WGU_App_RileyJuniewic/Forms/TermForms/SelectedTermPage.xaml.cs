using WGU_App_RileyJuniewic.Data.Misc.Events;
using WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;
using WGU_App_RileyJuniewic.Forms.CourseForms;
using WGU_App_RileyJuniewic.Forms.TermForms;

namespace WGU_App_RileyJuniewic.Forms;

public partial class SelectedTermPage : ContentPage, IQueryAttributable
{
    private Guid _termId;

    private readonly SelectedTermViewModel _viewModel;

    public EventHandler? ButtonEventHandler { get; set; }
    
    public SelectedTermPage(SelectedTermViewModel viewModel)
    {
        BindingContext = viewModel;
        _viewModel = viewModel;
        InitializeComponent();

        ButtonEventHandler += OnViewCourseEventHandler;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadDataCommand.Execute(null);
    }

    private void OnViewCourseEventHandler(object? sender, EventArgs e)
    {
        var handler = e as CourseEventArgs;
            if (handler is null) return;

        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "Course", handler.Course }
        };
        _ = Shell.Current.GoToAsync(nameof(ViewCoursePage), true, navigationParameter);
    }

    private void Add_Course_Clicked(object sender, EventArgs e)
	{
		if (_viewModel.Term is null) return;

		var navigationParameter = new ShellNavigationQueryParameters
		{
			{ "Term", _viewModel.Term }
		};
        _ = Shell.Current.GoToAsync(nameof(AddCoursePage), true, navigationParameter);
    }

    private void Add_Term_Clicked(object sender, EventArgs e)
    {
        _ = Shell.Current.GoToAsync(nameof(AddTermPage), true);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("TermId", out var termId) && termId is Guid guid)
        {
            _termId = guid;
            _viewModel.SetTerm(_termId);
        }
    }
}