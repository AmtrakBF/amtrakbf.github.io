using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels;

public class HomeViewModel : BindingModel
{
    private readonly ICourseService _courseService;
    private readonly IInstructorService _instructorService;
    private List<FullCourseDto> _fullCourses = [];

    public List<FullCourseDto> FullCourses
    {
        get => _fullCourses;
        set
        {
            _fullCourses = value;
            OnPropertyChanged(nameof(FullCourses));
        }
    }

    public OnClickCommandAsync LoadDataCommand { get; set; }

    public HomeViewModel(ICourseService courseService, IInstructorService instructorService)
    {
        _courseService = courseService;
        _instructorService = instructorService;

        LoadDataCommand = new(LoadDataAsync);

        _ = LoadDataAsync();
    }


    public async Task LoadDataAsync()
    {
        var courses = await _courseService.GetAllCoursesAsync();
        var instructors = await _instructorService.GetAllInstructorsAsync();

        var fullCourses = courses.Select(c => new FullCourseDto
        {
            Course = c,
            Instructor = instructors.FirstOrDefault(i => i.InstructorId == c.InstructorId)
        }).ToList();

        FullCourses = fullCourses;
        OnPropertyChanged(nameof(FullCourses));
    }
}