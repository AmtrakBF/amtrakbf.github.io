using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.AssessmentViewModels;

namespace WGU_App_RileyJuniewic.Forms.AssessmentForms;

public partial class ModifyAssessmentForm : ContentPage, IQueryAttributable
{
    private readonly ModifyAssessmentViewModel _viewModel;

    private Course _course = new();
    public Course Course
    {
        get => _course;
        set
        {
            _course = value;
            _viewModel.SetCourse(value);
            OnPropertyChanged(nameof(Course));
        }
    }

    public ModifyAssessmentForm()
    {
        _viewModel = ServiceHelper.GetService<ModifyAssessmentViewModel>();
        BindingContext = _viewModel;

        _viewModel.OnModify += GoBackEventHandler;
        _viewModel.OnDelete += GoBackEventHandler;

        InitializeComponent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Course", out var course) && course is Course)
        {
            Course = (Course)course;
        }
    }

    private void GoBackEventHandler(object? sender, EventArgs e) =>
        _ = Shell.Current.GoToAsync("..", true);
}