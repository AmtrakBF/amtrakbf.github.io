using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Dtos.Term;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Misc.Events;
using WGU_App_RileyJuniewic.Data.Services;

namespace WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;

public class AddTermViewModel : BindingModel
{
    private readonly ITermService _termService;

    private CreateTermRequest _createTermRequest = new();
    public CreateTermRequest CreateTermRequest
    {
        get => _createTermRequest;
        set
        {
            _createTermRequest = value;
            OnPropertyChanged(nameof(CreateTermRequest));
        }
    }

    public event EventHandler<ModifyTermEventArgs>? OnCreateTerm;
    public OnClickCommandAsync CreateTermCommandAsync { get; set; }
    public AddTermViewModel(ITermService termService)
    {
        _termService = termService;
        CreateTermCommandAsync = new OnClickCommandAsync(CreateTermAsync, (obj) => !CreateTermRequest.HasErrors);
        CreateTermRequest.PropertyChanged += (sender, args) => CreateTermCommandAsync.RaiseCanExecuteChanged();
    }

    
    public async Task CreateTermAsync()
    {
        var termResult = await _termService.CreateTermAsync(CreateTermRequest);
        if (termResult.IsError())
        {
            new UserError(termResult.Errors);
            return;
        }

        OnCreateTerm?.Invoke(this, new ModifyTermEventArgs(termResult.Value));
    }
    
}