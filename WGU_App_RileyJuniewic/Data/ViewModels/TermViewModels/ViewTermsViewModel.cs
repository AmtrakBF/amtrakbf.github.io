using System.ComponentModel;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Dtos.TermDtos;
using WGU_App_RileyJuniewic.Data.Models.Enums;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;

public class ViewTermsViewModel : BindingModel
{
    private readonly ITermService _termService;
    private readonly ICourseService _courseService;
    private BindingList<TermDisplayDto> _terms = [];
    public BindingList<TermDisplayDto> Terms
    {
        get => _terms;
        set
        {
            _terms = value;
            OnPropertyChanged(nameof(Terms));
        }
    }

    public ViewTermsViewModel(ITermService termService, ICourseService courseService)
    {
        _termService = termService;
        _courseService = courseService;
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var terms = await _termService.GetAllTermsAsync();
        var courses = await _courseService.GetAllCoursesAsync();

        var termCourses =
            terms.Select(term => new TermDisplayDto{
                TotalCourses = courses.Count(course => course.TermId == term.TermId),
                TotalCoursesFinished = courses.Count(course => course.TermId == term.TermId && course.Status == CourseStatus.Completed),
                TermId = term.TermId,
                Title = term.Title,
                StartDate = term.StartDate,
                EndDate = term.EndDate
            }).OrderBy(term => term.StartDate).ToList();

        Terms = new BindingList<TermDisplayDto>(termCourses);
    }
}