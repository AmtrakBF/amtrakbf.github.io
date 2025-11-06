using WGU_App_RileyJuniewic.Data.Misc.Commands;

namespace WGU_App_RileyJuniewic.Data.Models.Interfaces;

public interface ITermViewModel
{
    public OnClickCommandAsync LoadDataCommand { get; set; }
}