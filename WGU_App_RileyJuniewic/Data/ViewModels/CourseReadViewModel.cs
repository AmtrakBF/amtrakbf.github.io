using System.Collections.ObjectModel;
using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels.CourseViewModes;

public class ViewCourseViewModel : BindingModel
{
    private readonly ICourseService _courseService;
    private readonly IInstructorService _instructorService;
    private readonly IAssessmentService _assessmentService;
    private ObservableCollection<Assessment> _assessments = [];
    public ObservableCollection<Assessment> Assessments
    {
        get => _assessments;
        set
        {
            _assessments = value;
            OnPropertyChanged(nameof(Assessments));
        }
    }

    private FullCourseDto _fullCourse = new();

    public FullCourseDto FullCourse
    {
        get => _fullCourse;
        set
        {
            _fullCourse = value;
            OnPropertyChanged(nameof(FullCourse));
        }
    }

    public ViewCourseViewModel(
        ICourseService courseService,
        IInstructorService instructorService,
        IAssessmentService assessmentService)
    {
        _courseService = courseService;
        _instructorService = instructorService;
        _assessmentService = assessmentService;
    }

    public async Task SetCourseNotificationAsync(Guid courseId, int notificationStartId, int notificationEndId)
    {
        var result = await _courseService.SetNotificationAsync(courseId, notificationStartId, notificationEndId);
        if (result.IsError())
        {
            new ToastNotification(errors: result.Errors);
            return;
        }
        new ToastNotification("Course alert set", false);
        await LoadDataAsync(FullCourse.Course);
    }

    public async Task SetAssessmentNotificationAsync(Guid assessmentId, int notificationStartId, int notificationEndId)
    {
        var result = await _assessmentService.SetNotificationAsync(assessmentId, notificationStartId, notificationEndId);
        if (result.IsError())
        {
            new ToastNotification(errors: result.Errors);
            return;
        }
        new ToastNotification("Assessment alert set", false);
        Assessments = new ObservableCollection<Assessment>(Assessments);
        await LoadDataAsync(FullCourse.Course);
    }

    public async Task RemoveCourseNotificationAsync(Guid courseId)
    {
        var result = await _courseService.RemoveNotificationAsync(courseId);
        if (result.IsError())
        {
            new ToastNotification(errors: result.Errors);
            return;
        }
        new ToastNotification("Course alert removed", false);
        await LoadDataAsync(FullCourse.Course);
    }

    public async Task RemoveAssessmentNotificationAsync(Guid assessmentId)
    {
        var result = await _assessmentService.RemoveNotificationAsync(assessmentId);
        if (result.IsError())
        {
            new ToastNotification(errors: result.Errors);
            return;
        }
        new ToastNotification("Assessment alert removed", false);
        await LoadDataAsync(FullCourse.Course);
    }

    public async Task LoadDataAsync(Models.Course course)
    {
        var updatedCourse = await _courseService.GetCourseAsync(course.CourseId);
        if (updatedCourse.IsError())
        {
            new ToastNotification(errors: updatedCourse.Errors);
            return;
        }
        FullCourse.Course = updatedCourse.Value;

        var instructor = await _instructorService.GetInstructorAsync(updatedCourse.Value.InstructorId);
        if (instructor.IsError())
        {
            new ToastNotification(errors: instructor.Errors);
            return;
        }
        FullCourse.Instructor = instructor.Value;

        var assessments = await _assessmentService.GetAllAssessmentsAsync();
        Assessments = new ObservableCollection<Assessment>(assessments.Where(x => x.CourseId == course.CourseId));
    }
}