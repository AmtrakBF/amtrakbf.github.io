using System.ComponentModel.DataAnnotations;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;

namespace WGU_App_RileyJuniewic.Data.Dtos.Instructor;

public class UpdateInstructorRequest : CreateInstructorRequest, IRequestIdentity
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
    
    public UpdateInstructorRequest()
    {
        ValidateAll<UpdateInstructorRequest>();
    }
}