using WGU_App_RileyJuniewic.Data.Dtos.Term;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;

namespace WGU_App_RileyJuniewic.Data.ViewModels.TermViewModels;

public class UpdateTermViewModel : ModifyViewModelBase<UpdateTermRequest, Term>
{
    public UpdateTermViewModel(IModifyService<UpdateTermRequest, Term> modifyService) : base(modifyService)
    {
    }

    public void SetTerm(Term? term)
    {
        if (term == null)
            return;
            
        ModifyRequest.Id = term.TermId;
        ModifyRequest.Title = term.Title;
        ModifyRequest.StartDate = term.StartDate;
        ModifyRequest.EndDate = term.EndDate;
    }
}