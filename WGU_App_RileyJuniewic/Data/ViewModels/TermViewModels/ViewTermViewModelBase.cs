using System.Collections.ObjectModel;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;

public abstract class ViewTermViewModelBase : BindingModel
{
    protected readonly ICourseService _courseService;
    protected readonly IInstructorService _instructorService;
    protected readonly ITermService _termService;

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

    private bool _isRefreshing;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            _isRefreshing = value;
            OnPropertyChanged(nameof(IsRefreshing));
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

    public ViewTermViewModelBase(ICourseService courseService, IInstructorService instructorService, ITermService termService)
    {
        _courseService = courseService;
        _instructorService = instructorService;
        _termService = termService;
    }
}