using System.ComponentModel.DataAnnotations;
using WGU_App_RileyJuniewic.Data.Misc.Attributes;

namespace WGU_App_RileyJuniewic.Data.Dtos.Term;

public class CreateTermRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = "";

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    [DateTimeComparer("StartDate", true)]
    public DateTime EndDate { get; set; }
}