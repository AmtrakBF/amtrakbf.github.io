using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos.Course;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;
using WGU_App_RileyJuniewic.Data.Services;
using WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;

namespace WGU_App_RileyJuniewic.Data.ViewModels;

public class CurrentTermViewModel : ViewTermViewModelBase, ITermViewModel
{
    public OnClickCommandAsync LoadDataCommand { get; set; }

    public CurrentTermViewModel(ICourseService courseService, IInstructorService instructorService, ITermService termService)
        : base(courseService, instructorService, termService)
    {
        LoadDataCommand = new(LoadDataAsync);
        _ = LoadDataAsync();
    }


    public async Task LoadDataAsync()
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
        var currentTerm = terms.Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).FirstOrDefault();

        if (currentTerm is null)
        {
            new ToastNotification("No current term found.", true);
            IsRefreshing = false;
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
        IsRefreshing = false;
    }
}