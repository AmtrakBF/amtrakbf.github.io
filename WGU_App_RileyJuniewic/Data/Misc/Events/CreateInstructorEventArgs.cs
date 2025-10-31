using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Data.Misc.Events;

public class InstructorEventArgs : EventArgs
{

    public Instructor Instructor { get; set; }

    public InstructorEventArgs(Instructor instructor)
    {
        Instructor = instructor;
    }
}