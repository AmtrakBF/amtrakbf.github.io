using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;

public class SelectedTermViewModel : ViewTermViewModelBase, ITermViewModel
{
    private Guid _termId;
    public OnClickCommandAsync LoadDataCommand { get; set; }

    public SelectedTermViewModel(ICourseService courseService, IInstructorService instructorService, ITermService termService)
        : base(courseService, instructorService, termService)
    {
        LoadDataCommand = new(LoadDataAsync);
    }
    
    public void SetTerm(Guid termId)
    {
        _termId = termId;
        _ = LoadDataAsync();    
    }

    private async Task LoadDataAsync()
    {
        IsRefreshing = true;
        if (_termId == Guid.Empty)
        {
            IsRefreshing = false;
            return;
        }

        var currentTerm = await _termService.GetTermAsync(_termId);
        if (currentTerm.IsError())
        {
            new ToastNotification(currentTerm.Errors);
            IsRefreshing = false;
            return;
        }

        Term = currentTerm;

        var courses = await _courseService.GetAllCoursesAsync();
        var instructors = await _instructorService.GetAllInstructorsAsync();
        if (instructors.IsError())
        {
            new ToastNotification(instructors.Errors);
            IsRefreshing = false;
            return;
        }
        

        var fullCourses = courses.Where(x => x.TermId == Term.TermId).Select(c => new FullCourseDto
        {
            Course = c,
            Instructor = instructors.Value.FirstOrDefault(i => i.InstructorId == c.InstructorId) ?? new()
        }).ToList();

        FullCourses = new(fullCourses);
        EmptyCoursesList = FullCourses.Count == 0;
        OnPropertyChanged(nameof(FullCourses));
        IsRefreshing = false;
    }
}