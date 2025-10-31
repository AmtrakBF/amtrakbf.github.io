using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Data.Misc.Events;

public class CreateInstructorEventArgs : EventArgs
{

    public Instructor Instructor { get; set; }

    public CreateInstructorEventArgs(Instructor instructor)
    {
        Instructor = instructor;
    }
}