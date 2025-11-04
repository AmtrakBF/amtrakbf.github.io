using WGU_App_RileyJuniewic.Data.Dtos.Assessment;
using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Data.ViewModels.AssessmentViewModels;

public class ModifyAssessmentViewModel : ModifyViewModelBase<UpdateAssessmentRequest, Assessment>
{
    public ModifyAssessmentViewModel(Models.Interfaces.IModifyService<UpdateAssessmentRequest, Assessment> modifyService) : base(modifyService)
    {
    }

    public void SetCourse(Models.Course course)
    {
        ModifyRequest.CourseId = course.CourseId;
    }
}