using System.Security.Principal;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;

namespace WGU_App_RileyJuniewic.Data.Dtos.Term;

public class UpdateTermRequest : CreateTermRequest, IRequestIdentity
{
    private Guid _id;
    public Guid Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged(nameof(Id));
            Validate(nameof(Id), _id);
        }
    }

    public UpdateTermRequest()
    {
        ValidateAll<UpdateTermRequest>();
    }
}