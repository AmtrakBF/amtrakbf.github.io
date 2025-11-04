using WGU_App_RileyJuniewic.Data.Misc.Attributes;

namespace WGU_App_RileyJuniewic.Data.Dtos.Course;

public class UpdateCourseRequest : CreateCourseRequest
{
    protected Guid _courseId;
    [RequireNonDefault]
    public Guid CourseId
    {
        get => _courseId;
        set
        {
            _courseId = value;
            OnPropertyChanged(nameof(CourseId));
            Validate(nameof(CourseId), _courseId);
        }
    }

    public UpdateCourseRequest()
    {
        ValidateAll<UpdateCourseRequest>();
    }
}