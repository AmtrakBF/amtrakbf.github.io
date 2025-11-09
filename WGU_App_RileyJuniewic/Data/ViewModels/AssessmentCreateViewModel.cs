using WGU_App_RileyJuniewic.Data.Dtos.Assessment;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;

namespace WGU_App_RileyJuniewic.Data.ViewModels.AssessmentViewModels;

public class CreateAssessmentViewModel : CreateViewModelBase<CreateAssessmentRequest, Assessment>
{
    public CreateAssessmentViewModel(ICreateService<CreateAssessmentRequest, Assessment> createService) : base(createService)
    {
    }

    public void SetCourse(Models.Course course)
    {
        CreateRequest.CourseId = course.CourseId;
    }
}