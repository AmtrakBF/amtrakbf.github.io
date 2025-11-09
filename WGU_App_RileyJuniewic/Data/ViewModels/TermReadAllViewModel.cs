using System.Collections.ObjectModel;
using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Dtos.TermDtos;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models.Enums;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;

public class ViewAllTermsViewModel : BindingModel
{
    private readonly ITermService _termService;
    private readonly ICourseService _courseService;

    private ObservableCollection<TermDisplayDto> _terms = [];
    public ObservableCollection<TermDisplayDto> Terms
    {
        get => _terms;
        set
        {
            _terms = value;
            OnPropertyChanged(nameof(Terms));
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

    private bool _emptyTermsList = false;
    public bool EmptyTermsList
    {
        get => _emptyTermsList;
        set
        {
            _emptyTermsList = value;
            OnPropertyChanged(nameof(EmptyTermsList));
        }
    }

    public Command LoadDataCommand { get; set; }

    public ViewAllTermsViewModel(ITermService termService, ICourseService courseService)
    {
        _termService = termService;
        _courseService = courseService;
        _ = LoadDataAsync();

        LoadDataCommand = new(async () => await LoadDataAsync());
    }

    private async Task LoadDataAsync()
    {
        IsRefreshing = true;

        var termRequest = await _termService.GetAllTermsAsync();
         if (termRequest.IsError())
        {
            new ToastNotification(termRequest.Errors);
            IsRefreshing = false;
            return;
        }
        var terms = termRequest.Value;
        var courses = await _courseService.GetAllCoursesAsync();

        var termCourses =
            terms.Select(term => new TermDisplayDto
            {
                TotalCourses = courses.Count(course => course.TermId == term.TermId),
                TotalCoursesFinished = courses.Count(course => course.TermId == term.TermId && course.Status == CourseStatus.Completed),
                TermId = term.TermId,
                Title = term.Title,
                StartDate = term.StartDate,
                EndDate = term.EndDate
            }).OrderBy(term => term.StartDate).ToList();

        Terms = new ObservableCollection<TermDisplayDto>(termCourses);
        EmptyTermsList = Terms.Count == 0;

        IsRefreshing = false;
    }
}