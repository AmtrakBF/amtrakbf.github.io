using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.CourseViewModes;

namespace WGU_App_RileyJuniewic.Forms.CourseForms;

public sealed partial class ViewCoursePage : ContentPage, IQueryAttributable
{
    private ViewCourseViewModel _viewModel;

    private Course _course = new();
    public Course Course
    {
        get => _course;
        set
        {
            _course = value;
            _ = _viewModel.LoadDataAsync(value);
            OnPropertyChanged(nameof(Course));
        }
    }

    public EventHandler? OnModifyEvent { get; set; }

    public ViewCoursePage()
    {
        _viewModel = ServiceHelper.GetService<ViewCourseViewModel>();
        BindingContext = _viewModel;

        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _viewModel.LoadDataAsync(Course);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Course", out var courseValue) && courseValue is Course course)
        {
            Course = course;
        }
    }
    
}