using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.ViewModels.CourseViewModes;

namespace WGU_App_RileyJuniewic.Forms.InstructorForms;

public sealed partial class ModifyInstructorPage : ContentPage, IQueryAttributable
{
    private ModifyCourseViewModel _viewModel;

    private FullCourseDto _fullCourseDto = new();
    public FullCourseDto FullCourseDto
    {
        get => _fullCourseDto;
        set
        {
            _fullCourseDto = value;
            _viewModel.SetCourse(value.Course);
            OnPropertyChanged(nameof(FullCourseDto));
        }
    }

    private Instructor? _selectedInstructor = new();
    public Instructor? SelectedInstructor
    {
        get => _selectedInstructor;
        set
        {
            _selectedInstructor = value;
            FullCourseDto.Course.InstructorId = value?.InstructorId ?? Guid.Empty;
            _viewModel.SetCourse(_fullCourseDto.Course);
            OnPropertyChanged(nameof(SelectedInstructor));
        }
    }


    public ModifyInstructorPage()
    {
        _viewModel = ServiceHelper.GetService<ModifyCourseViewModel>();
        BindingContext = _viewModel;
        _viewModel.OnModify += (sender, e) => _ = Shell.Current.GoToAsync("..", true);

        InitializeComponent();
    }
    

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("FullCourse", out var fullCourseValue) && fullCourseValue is FullCourseDto fullCourseDto)
        {
            FullCourseDto = fullCourseDto;
            SelectedInstructor = fullCourseDto.Instructor;
        }
    }
}