using WGU_App_RileyJuniewic.Data.Dtos.Instructor;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;

namespace WGU_App_RileyJuniewic.Data.ViewModels.InstructorViewModels;

public class AddInstructorViewModel : CreateViewModelBase<CreateInstructorRequest, Instructor>
{
    public AddInstructorViewModel(ICreateService<CreateInstructorRequest, Instructor> createService) : base(createService)
    {
    }
}