using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Misc.Commands;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;

namespace WGU_App_RileyJuniewic.Data.ViewModels;

public abstract class CreateViewModelBase<TRequest, TResponse> : BindingModel
    where TRequest : BindingModel, new()
    where TResponse : class
{
    protected readonly ICreateService<TRequest, TResponse> _createService;

    private TRequest _createRequest = new();
    public TRequest CreateRequest
    {
        get => _createRequest;
        set
        {
            _createRequest = value;
            OnPropertyChanged(nameof(CreateRequest));
        }
    }

    public virtual event EventHandler<EventArgs>? OnCreate;
    public OnClickCommandAsync CreateCommandAsync { get; set; }

    protected CreateViewModelBase(ICreateService<TRequest, TResponse> createService)
    {
        _createService = createService;
        CreateCommandAsync = new OnClickCommandAsync(CreateAsync, (obj) => !CreateRequest.HasErrors);
        CreateRequest.PropertyChanged += (sender, args) => CreateCommandAsync.RaiseCanExecuteChanged();
    }

    protected virtual async Task CreateAsync()
    {
        if (CreateRequest.HasErrors)
        {
            var errors = CreateRequest.GetAllErrors();
            new ToastNotification(errors: errors);
            return;
        }

        var result = await _createService.CreateAsync(CreateRequest);
        if (result.IsError())
        {
            new ToastNotification(result.Errors);
            return;
        }

        OnCreate?.Invoke(this, EventArgs.Empty);
    }
}