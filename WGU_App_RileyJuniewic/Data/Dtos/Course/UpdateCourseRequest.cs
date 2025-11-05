using WGU_App_RileyJuniewic.Data.Misc.Attributes;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;

namespace WGU_App_RileyJuniewic.Data.Dtos.Course;

public class UpdateCourseRequest : CreateCourseRequest, IRequestIdentity
{
    protected Guid _id;
    [RequireNonDefault]
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

    public UpdateCourseRequest()
    {
        ValidateAll<UpdateCourseRequest>();
    }
}