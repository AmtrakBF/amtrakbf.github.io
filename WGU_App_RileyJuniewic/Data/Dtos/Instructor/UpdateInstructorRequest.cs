using System.ComponentModel.DataAnnotations;

namespace WGU_App_RileyJuniewic.Data.Dtos.Instructor;

public class UpdateInstructorRequest : CreateInstructorRequest
{
    protected Guid _instructorId;
    [Required]
    public Guid InstructorId
    {
        get => _instructorId;
        set
        {
            _instructorId = value;
            OnPropertyChanged(nameof(InstructorId));
            Validate(nameof(InstructorId), _instructorId);
        }
    }
    
    public UpdateInstructorRequest()
    {
        ValidateAll<UpdateInstructorRequest>();
    }
}