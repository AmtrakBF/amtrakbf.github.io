using WGU_App_RileyJuniewic.Data.Dtos;
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
        Instructors = (await _instructorService.GetAllInstructorsAsync()).OrderBy(x => x.Name).ToList();
    }
    
}