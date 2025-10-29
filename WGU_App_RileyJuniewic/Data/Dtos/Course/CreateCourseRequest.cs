using System.ComponentModel.DataAnnotations;
using WGU_App_RileyJuniewic.Data.Misc.Attributes;
using WGU_App_RileyJuniewic.Data.Models.Enums;

namespace WGU_App_RileyJuniewic.Data.Dtos.Course;

public class CreateCourseRequest
{
    [Required]
    public Guid TermId { get; set; }

    public Guid InstructorId { get; set; }

    [Required]
    public string Title { get; set; } = "";

    [Required]
    public CourseStatus Status { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    [DateTimeComparer(nameof(StartDate), true)]
    public DateTime EndDate { get; set; }
}