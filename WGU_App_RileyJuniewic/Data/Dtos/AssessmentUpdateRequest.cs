using System.ComponentModel.DataAnnotations;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;

namespace WGU_App_RileyJuniewic.Data.Dtos.Assessment;

public class UpdateAssessmentRequest : CreateAssessmentRequest, IRequestIdentity
{
    protected Guid _id;
    [Required]
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

    public UpdateAssessmentRequest()
    {
        ValidateAll<UpdateAssessmentRequest>();
    }    
}