using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models.Enums;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels;

public class ReportsViewModel : BindingModel
{
    private ReportDto _reportDto = new();
    private readonly ICourseService _courseService;
    private readonly IAssessmentService _assessmentService;
    private readonly ITermService _termService;

    public ReportDto ReportDto
    {
        get => _reportDto;
        set => SetValue(nameof(ReportDto), ref _reportDto, value);
    }

    public ReportsViewModel(ICourseService courseService,
                            IAssessmentService assessmentService,
                            ITermService termService)
    {
        _courseService = courseService;
        _assessmentService = assessmentService;
        _termService = termService;

        _ = LoadReports();
    }

    public async Task LoadReports()
    {
        var courses = await _courseService.GetAllCoursesAsync();
        var assessments = await _assessmentService.GetAllAssessmentsAsync();
        var termsResult = await _termService.GetAllTermsAsync();
        if (termsResult.IsError())
        {
            new ToastNotification(termsResult.Errors);
            return;
        }

        ReportDto = new ReportDto
        {
            CompleteCourseCount = courses.Count(x => x.Status == CourseStatus.Completed),
            DroppedCourseCount = courses.Count(x => x.Status == CourseStatus.Dropped),
            ActiveCourseCount = courses.Count(x => x.Status == CourseStatus.Active),
            PlannedCourseCount = courses.Count(x => x.Status == CourseStatus.Planned),
            CompleteAssessmentCount = assessments.Count(x => x.EndDate < DateTime.Now),
            IncompleteAssessmentCount = assessments.Count(x => x.EndDate > DateTime.Now),
            TotalCourseCount = courses.Count,
            TotalAssessmentCount = assessments.Count,
            TotalTermCount = termsResult.Value.Count,
            CompleteTermCount = termsResult.Value.Count(x => x.EndDate < DateTime.Now),
            ReportDate = DateTime.Now
        };
    }
}