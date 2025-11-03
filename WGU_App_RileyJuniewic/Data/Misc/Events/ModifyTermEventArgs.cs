using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Data.Misc.Events;

public class ModifyTermEventArgs : EventArgs
{
    public Term Term { get; set; }

    public ModifyTermEventArgs(Term term)
    {
        Term = term;
    }
}