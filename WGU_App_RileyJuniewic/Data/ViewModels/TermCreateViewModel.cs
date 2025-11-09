using WGU_App_RileyJuniewic.Data.Dtos.Term;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;

namespace WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;

public class AddTermViewModel : CreateViewModelBase<CreateTermRequest, Term>
{
    public AddTermViewModel(ICreateService<CreateTermRequest, Term> createService) : base(createService)
    {
    }
}