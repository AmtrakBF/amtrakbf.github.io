using WGU_App_RileyJuniewic.Data.Dtos.Instructor;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;

namespace WGU_App_RileyJuniewic.Data.ViewModels.InstructorViewModels;

public class UpdateInstructorViewModel : ModifyViewModelBase<UpdateInstructorRequest, Instructor>
{
    public UpdateInstructorViewModel(IModifyService<UpdateInstructorRequest, Instructor> modifyService) : base(modifyService)
    {
    }

    public void SetInstructor(Instructor? instructor)
    {
        if (instructor == null)
            return;
            
        ModifyRequest.Id = instructor.InstructorId;
        ModifyRequest.Name = instructor.Name;
        ModifyRequest.Phone = instructor.Phone;
        ModifyRequest.Email = instructor.Email;
    }

}