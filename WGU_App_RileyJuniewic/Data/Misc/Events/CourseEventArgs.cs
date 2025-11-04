using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Data.Misc.Events;

public class CourseEventArgs : EventArgs
{
    public Course Course { get; set; }

    public CourseEventArgs(Course course)
    {
        Course = course;
    }
}