using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.CourseViewModes;

namespace WGU_App_RileyJuniewic.Forms.CourseForms;

public sealed partial class ModifyCoursePage : ContentPage, IQueryAttributable
{
    private ModifyCourseViewModel _viewModel;

    private Course course = new();
    public Course Course
    {
        get => course;
        set
        {
            course = value;
            _viewModel.SetCourse(value);
            OnPropertyChanged(nameof(Course));
        }
    }

    public ModifyCoursePage()
    {
        _viewModel = ServiceHelper.GetService<ModifyCourseViewModel>();
        BindingContext = _viewModel;

        _viewModel.OnDelete += OnDeleteEventHandler;
        _viewModel.OnModify += OnModifyEventHandler;

        InitializeComponent();
    }

    public void OnModifyEventHandler(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..", true);
    }

    private void OnDeleteEventHandler(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"//{nameof(CurrentTermPage)}", true);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Course", out var courseValue) && courseValue is Course course)
        {
            Course = course;
        }
    }

    private void OnCancelEventHandler(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..", true);
    }
}