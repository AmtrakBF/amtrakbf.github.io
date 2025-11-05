using WGU_App_RileyJuniewic.Data.Dtos.Assessment;
using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Data.ViewModels.AssessmentViewModels;

public class ModifyAssessmentViewModel : ModifyViewModelBase<UpdateAssessmentRequest, Assessment>
{
    public ModifyAssessmentViewModel(Models.Interfaces.IModifyService<UpdateAssessmentRequest, Assessment> modifyService) : base(modifyService)
    {
    }

    public void SetAssessment(Assessment assessment)
    {
        ModifyRequest.Id = assessment.AssessmentId;
        ModifyRequest.CourseId = assessment.CourseId;
        ModifyRequest.Name = assessment.Name;
        ModifyRequest.Type = assessment.Type.ToString();
        ModifyRequest.StartDate = assessment.StartDate;
        ModifyRequest.EndDate = assessment.EndDate;
    }
}