using System.Diagnostics;
using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Dtos.Instructor;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Misc.Events;
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

    private CreateInstructorRequest _createInstructorRequest;
    public CreateInstructorRequest CreateInstructorRequest
    {
        get => _createInstructorRequest;
        set
        {
            _createInstructorRequest = value;
            OnPropertyChanged(nameof(CreateInstructorRequest));
        }
    }

    public OnClickCommandAsync CreateInstructorCommandAsync { get; set; }
    public event EventHandler<CreateInstructorEventArgs>? OnCreateInstructor;

    public InstructorCardViewModel(IInstructorService instructorService)
    {
        _createInstructorRequest = new();
        _instructorService = instructorService;

        CreateInstructorCommandAsync = new OnClickCommandAsync(CreateInstructorAsync, (obj) => !CreateInstructorRequest.HasErrors);
        CreateInstructorRequest.PropertyChanged += (sender, args) => CreateInstructorCommandAsync.RaiseCanExecuteChanged();

        _ = GetAllInstructorsAsync();
    }

    private async Task GetAllInstructorsAsync()
    {
        Instructors = (await _instructorService.GetAllInstructorsAsync()).ToList();
        CreateInstructorCommandAsync.RaiseCanExecuteChanged();
    }

    private async Task CreateInstructorAsync()
    {
        var instructorResult = await _instructorService.CreateInstructorAsync(CreateInstructorRequest);
        if (instructorResult.IsError())
        {
            new UserError(instructorResult.Errors);
            return;
        }

        CreateInstructorRequest = new();
        CreateInstructorRequest.PropertyChanged += (sender, args) => CreateInstructorCommandAsync.RaiseCanExecuteChanged();

        await GetAllInstructorsAsync();
        OnCreateInstructor?.Invoke(this, new CreateInstructorEventArgs(instructorResult.Value));
    }
}