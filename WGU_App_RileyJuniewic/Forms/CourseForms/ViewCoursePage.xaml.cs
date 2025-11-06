using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.CourseViewModes;
using WGU_App_RileyJuniewic.Forms.AssessmentForms;
using WGU_App_RileyJuniewic.Forms.InstructorForms;

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

    public EventHandler? OnModifyCourseEvent { get; set; }

    public ViewCoursePage()
    {
        _viewModel = ServiceHelper.GetService<ViewCourseViewModel>();
        BindingContext = _viewModel;

        OnModifyCourseEvent += ModifyCourseEventHandler;

        InitializeComponent();
    }

    public void ModifyInstructorEventHandler(object sender, EventArgs e)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "FullCourse", _viewModel.FullCourse }
        };
        _ = Shell.Current.GoToAsync(nameof(ModifyInstructorPage), true, navigationParameter);
    }

    public void ModifyCourseEventHandler(object? sender, EventArgs e)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "Course", Course }
        };
        _ = Shell.Current.GoToAsync(nameof(ModifyCoursePage), true, navigationParameter);
    }

    public void AddAssessmentEventHandler(object? sender, EventArgs e)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "Course", Course }
        };
        _ = Shell.Current.GoToAsync(nameof(CreateAssessmentPage), true, navigationParameter);
    }

    public void ModifyAssessessmentEventHandler(object? sender, EventArgs e)
    {
        var senderButton = sender as Button;
        var assessment = senderButton?.BindingContext as Assessment;
        if (assessment is null)
        {
            new UserError("Cannot modify assessment");
            return;
        }

        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "Assessment", assessment }
        };
        _ = Shell.Current.GoToAsync(nameof(ModifyAssessmentPage), true, navigationParameter);
    }

    public void ShareNotesEventHandler(object sender, EventArgs e)
    {
        _ = Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = _viewModel.FullCourse.Course.Notes,
            Title = "Share Notes"
        });
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Course", out var courseValue) && courseValue is Course course)
        {
            Course = course;
        }
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _viewModel.LoadDataAsync(Course);
    }
}