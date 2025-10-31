using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Dtos.Instructor;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Misc.Events;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels.InstructorViewModels;

public class AddInstructorViewModel : BindingModel
{
    private readonly IInstructorService _instructorService;

    private CreateInstructorRequest _createInstructorRequest = new();
    public CreateInstructorRequest CreateInstructorRequest
    {
        get => _createInstructorRequest;
        set
        {
            _createInstructorRequest = value;
            OnPropertyChanged(nameof(CreateInstructorRequest));
        }
    }

    public event EventHandler<InstructorEventArgs>? OnCreateInstructor;
    public OnClickCommandAsync CreateInstructorCommandAsync { get; set; }

    public AddInstructorViewModel(IInstructorService instructorService)
    {
        _instructorService = instructorService;

        CreateInstructorCommandAsync = new OnClickCommandAsync(CreateInstructorAsync, (obj) => !CreateInstructorRequest.HasErrors);
        CreateInstructorRequest.PropertyChanged += (sender, args) => CreateInstructorCommandAsync.RaiseCanExecuteChanged();
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

        OnCreateInstructor?.Invoke(this, new InstructorEventArgs(instructorResult.Value));
    }
}