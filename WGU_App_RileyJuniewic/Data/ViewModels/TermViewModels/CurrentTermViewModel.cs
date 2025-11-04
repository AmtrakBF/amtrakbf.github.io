using System.Collections.ObjectModel;
using System.ComponentModel;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels;

public class CurrentTermViewModel : BindingModel
{
    private readonly ICourseService _courseService;
    private readonly IInstructorService _instructorService;
    private readonly ITermService _termService;
    
    private Term? _term;
    public Term? Term
    {
        get => _term;
        set
        {
            _term = value;
            OnPropertyChanged(nameof(Term));
        }
    }

    private ObservableCollection<FullCourseDto> _fullCourses = [];
    public ObservableCollection<FullCourseDto> FullCourses
    {
        get => _fullCourses;
        set
        {
            _fullCourses = value;
            OnPropertyChanged(nameof(FullCourses));
        }
    }

    private bool _emptyCoursesList = false;
    public bool EmptyCoursesList
    {
        get => _emptyCoursesList;
        set
        {
            _emptyCoursesList = value;
            OnPropertyChanged(nameof(EmptyCoursesList));
        }
    }

    public OnClickCommandAsync LoadDataCommand { get; set; }

    public CurrentTermViewModel(ICourseService courseService, IInstructorService instructorService, ITermService termService)
    {
        _courseService = courseService;
        _instructorService = instructorService;
        _termService = termService;
        LoadDataCommand = new(LoadDataAsync);

        _ = LoadDataAsync();
    }


    public async Task LoadDataAsync()
    {
        var terms = await _termService.GetAllTermsAsync();
        var currentTerm = terms.Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).FirstOrDefault();

        if (currentTerm is null)
        {
            new UserError("No current term found.");
            return;
        }

        Term = currentTerm;

        var courses = await _courseService.GetAllCoursesAsync();
        var instructors = await _instructorService.GetAllInstructorsAsync();

        var fullCourses = courses.Where(x => x.TermId == Term.TermId).Select(c => new FullCourseDto
        {
            Course = c,
            Instructor = instructors.FirstOrDefault(i => i.InstructorId == c.InstructorId) ?? new()
        }).ToList();

        FullCourses = new(fullCourses);
        EmptyCoursesList = FullCourses.Count == 0;
        OnPropertyChanged(nameof(FullCourses));
    }
}