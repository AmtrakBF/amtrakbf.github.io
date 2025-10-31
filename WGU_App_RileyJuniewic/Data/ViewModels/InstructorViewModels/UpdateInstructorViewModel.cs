using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Dtos.Instructor;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Misc.Events;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels.InstructorViewModels;

public class UpdateInstructorViewModel : BindingModel
{
    private UpdateInstructorRequest _updateInstructorRequest = new();
    private readonly IInstructorService _instructorService;

    public UpdateInstructorRequest UpdateInstructorRequest
    {
        get => _updateInstructorRequest;
        set
        {
            _updateInstructorRequest = value;
            OnPropertyChanged(nameof(UpdateInstructorRequest));
        }
    }

    public OnClickCommandAsync UpdateInstructorCommandAsync { get; set; }
    public event EventHandler<InstructorEventArgs>? OnUpdateInstructor;
    
    public OnClickCommandAsync DeleteInstructorCommandAsync { get; set; }
    public event EventHandler? OnDeleteInstructor;

    public UpdateInstructorViewModel(IInstructorService instructorService)
    {
        _instructorService = instructorService;

        UpdateInstructorCommandAsync = new OnClickCommandAsync(UpdateInstructorAsync, (obj) => !UpdateInstructorRequest.HasErrors);
        UpdateInstructorRequest.PropertyChanged += (sender, args) => UpdateInstructorCommandAsync.RaiseCanExecuteChanged();

        DeleteInstructorCommandAsync = new OnClickCommandAsync(DeleteInstructorAsync);
    }

    public void SetInstructor(Instructor? instructor)
    {
        if (instructor == null)
            return;
            
        UpdateInstructorRequest.InstructorId = instructor.InstructorId;
        UpdateInstructorRequest.Name = instructor.Name;
        UpdateInstructorRequest.Phone = instructor.Phone;
        UpdateInstructorRequest.Email = instructor.Email;
    }

    private async Task UpdateInstructorAsync()
    {
        var instructorResult = await _instructorService.UpdateInstructorAsync(UpdateInstructorRequest);
        if (instructorResult.IsError())
        {
            new UserError(instructorResult.Errors);
            return;
        }

        ResetRequest();
        OnUpdateInstructor?.Invoke(this, new InstructorEventArgs(instructorResult.Value));
    }

    private async Task DeleteInstructorAsync()
    {
        await _instructorService.DeleteInstructorAsync(UpdateInstructorRequest.InstructorId);
        ResetRequest();
        OnDeleteInstructor?.Invoke(this, new());
    }

    private void ResetRequest()
    {
        UpdateInstructorRequest = new();
        UpdateInstructorRequest.PropertyChanged += (sender, args) => UpdateInstructorCommandAsync.RaiseCanExecuteChanged();
    }
}