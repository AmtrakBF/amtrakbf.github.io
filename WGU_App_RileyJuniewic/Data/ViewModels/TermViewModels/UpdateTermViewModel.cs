using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Dtos.Term;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Misc.Events;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;

public class UpdateTermViewModel : BindingModel
{
    private readonly ITermService _termService;

    private UpdateTermRequest _updateTermRequest = new();
    public UpdateTermRequest UpdateTermRequest
    {
        get => _updateTermRequest;
        set
        {
            _updateTermRequest = value;
            OnPropertyChanged(nameof(UpdateTermRequest));
        }
    }

    public OnClickCommandAsync UpdateTermCommandAsync { get; set; }
    public event EventHandler<ModifyTermEventArgs>? OnUpdateTerm;
    
    public OnClickCommandAsync DeleteTermCommandAsync { get; set; }
    public event EventHandler<ModifyTermEventArgs>? OnDeleteTerm;

    public UpdateTermViewModel(ITermService termService)
    {
        _termService = termService;

        UpdateTermCommandAsync = new OnClickCommandAsync(UpdateTermAsync, (obj) => !UpdateTermRequest.HasErrors);
        UpdateTermRequest.PropertyChanged += (sender, args) => UpdateTermCommandAsync.RaiseCanExecuteChanged();

        DeleteTermCommandAsync = new OnClickCommandAsync(DeleteInstructorAsync);
    }

    public void SetTerm(Term? term)
    {
        if (term == null)
            return;
            
        UpdateTermRequest.TermId = term.TermId;
        UpdateTermRequest.Title = term.Title;
        UpdateTermRequest.StartDate = term.StartDate;
        UpdateTermRequest.EndDate = term.EndDate;
    }

    private async Task UpdateTermAsync()
    {
        var termResult = await _termService.UpdateTermAsync(UpdateTermRequest);
        if (termResult.IsError())
        {
            new UserError(termResult.Errors);
            return;
        }

        OnUpdateTerm?.Invoke(this, new(termResult.Value));
    }

    private async Task DeleteInstructorAsync()
    {
        var deletedTerm = await _termService.GetTermAsync(UpdateTermRequest.TermId);
        if (deletedTerm.IsError())
        {
            new UserError(deletedTerm.Errors);
            return;
        }

        await _termService.DeleteTermAsync(UpdateTermRequest.TermId);
        OnDeleteTerm?.Invoke(this, new(deletedTerm.Value));
    }

}