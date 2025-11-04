using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.AssessmentViewModels;

namespace WGU_App_RileyJuniewic.Forms.AssessmentForms;

public partial class CreateAssessmentForm : ContentPage, IQueryAttributable
{
    private readonly CreateAssessmentViewModel _viewModel;

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
    
    public CreateAssessmentForm()
    {
        _viewModel = ServiceHelper.GetService<CreateAssessmentViewModel>();
        BindingContext = _viewModel;

        _viewModel.OnCreate += GoBackEventHandler;

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