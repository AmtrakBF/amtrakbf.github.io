using System.ComponentModel.DataAnnotations;

namespace WGU_App_RileyJuniewic.Data.Dtos.Assessment;

public class UpdateAssessmentRequest : CreateAssessmentRequest
{
    protected Guid _assessmentId;
    [Required]
    public Guid AssessmentId
    {
        get => _assessmentId;
        set
        {
            _assessmentId = value;
            OnPropertyChanged(nameof(AssessmentId));
            Validate(nameof(AssessmentId), _assessmentId);
        }
    }

    public UpdateAssessmentRequest()
    {
        ValidateAll<UpdateAssessmentRequest>();
    }    
}