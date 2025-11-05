using WGU_App_RileyJuniewic.Data.Dtos.Assessment;
using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Data.Misc.Events;

public class CreateAssessmentEventArgs : EventArgs
{
    public CreateAssessmentRequest Request { get; set; }

    public CreateAssessmentEventArgs(CreateAssessmentRequest request) 
    {
        this.Request = request;
    }
}