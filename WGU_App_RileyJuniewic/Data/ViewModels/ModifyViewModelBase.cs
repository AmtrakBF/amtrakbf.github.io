using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;

namespace WGU_App_RileyJuniewic.Data.ViewModels;

public class ModifyViewModelBase<TRequest, TResponse> : BindingModel
    where TRequest : BindingModel, IRequestIdentity, new()
    where TResponse : class
{
    private readonly IModifyService<TRequest, TResponse> _modifyService;

    private TRequest _modifyRequest = new();
    public TRequest ModifyRequest
    {
        get => _modifyRequest;
        set
        {
            _modifyRequest = value;
            OnPropertyChanged(nameof(ModifyRequest));
        }
    }

    public event EventHandler<EventArgs>? OnModify;
    public event EventHandler<EventArgs>? OnDelete;
    public OnClickCommandAsync ModifyCommandAsync { get; set; }
    public OnClickCommandAsync DeleteCommandAsync { get; set; }

    public ModifyViewModelBase(IModifyService<TRequest, TResponse> modifyService)
    {
        _modifyService = modifyService;

        ModifyCommandAsync = new OnClickCommandAsync(ModifyAsync, (obj) => !ModifyRequest.HasErrors);
        DeleteCommandAsync = new OnClickCommandAsync(DeleteAsync);
        
        ModifyRequest.PropertyChanged += (sender, args) => ModifyCommandAsync.RaiseCanExecuteChanged();
    }

    private async Task DeleteAsync()
    {
        var result = await _modifyService.DeleteAsync(ModifyRequest.Id);
        if (result.IsError())
        {
            new UserError(result.Errors);
            return;
        }

        OnDelete?.Invoke(this, EventArgs.Empty);
    }

    private async Task ModifyAsync()
    {
        var result = await _modifyService.UpdateAsync(ModifyRequest);
        if (result.IsError())
        {
            new UserError(result.Errors);
            return;
        }

        OnModify?.Invoke(this, EventArgs.Empty);
    }
}