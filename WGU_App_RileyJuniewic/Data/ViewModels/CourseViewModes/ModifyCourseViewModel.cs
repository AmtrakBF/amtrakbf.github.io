using WGU_App_RileyJuniewic.Data.Dtos.Course;

namespace WGU_App_RileyJuniewic.Data.ViewModels.CourseViewModes;

public class ModifyCourseViewModel : ModifyViewModelBase<UpdateCourseRequest, Models.Course>
{
    public ModifyCourseViewModel(Models.Interfaces.IModifyService<UpdateCourseRequest, Models.Course> modifyService) : base(modifyService)
    {
    }

    public void SetCourse(Models.Course course)
    {
        ModifyRequest = new UpdateCourseRequest()
        {
            Id = course.CourseId,
            TermId = course.TermId,
            InstructorId = course.InstructorId,
            Title = course.Title,
            Notes = course.Notes,
            Status = course.Status.ToString(),
            StartDate = course.StartDate,
            EndDate = course.EndDate
        };
    }
}