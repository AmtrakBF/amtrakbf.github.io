using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels.Course;

public class InstructorCardViewModel : BindingModel
{
    private readonly IInstructorService _instructorService;

    private List<Instructor> _instructors = new();
    public List<Instructor> Instructors
    {
        get => _instructors;
        set
        {
            _instructors = value;
            OnPropertyChanged(nameof(Instructors));
        }
    }

    public InstructorCardViewModel(IInstructorService instructorService)
    {
        _instructorService = instructorService;

        _ = GetAllInstructorsAsync();
    }

    public async Task GetAllInstructorsAsync()
    {
        var result = await _instructorService.GetAllInstructorsAsync();
        if (result.IsError())
        {
            new ToastNotification(result.Errors);
            return;
        }
        Instructors = result.Value.OrderBy(x => x.Name).ToList();
    }
    
}