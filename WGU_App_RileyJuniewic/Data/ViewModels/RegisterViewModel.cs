using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Misc.Attributes.Exceptions;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;

namespace WGU_App_RileyJuniewic.Data.ViewModels;

public class RegisterViewModel : CreateViewModelBase<UserCreateRequest, User>
{
    private readonly UserStore _userStore;

    public RegisterViewModel(ICreateService<UserCreateRequest, User> createService, UserStore userStore) : base(createService)
    {
        _userStore = userStore;
    }

    public event EventHandler? RegisterEvent;

    protected override async Task CreateAsync()
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

        _userStore.SetUser(result.Value);
        RegisterEvent?.Invoke(this, EventArgs.Empty);
    }
}