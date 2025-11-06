using SQLite;
using WGU_App_RileyJuniewic.Data.Models.Enums;

namespace WGU_App_RileyJuniewic.Data.Models;

[Table("Course")]
public class Course
{
    [PrimaryKey]
    public Guid CourseId { get; set; }

    [Indexed]
    public Guid TermId { get; set; }

    [Indexed]
    public Guid InstructorId { get; set; }

    public string Title { get; set; } = "";
    public string? Notes { get; set; }
    public CourseStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NotificationStartId { get; set; } = -1;
    public int NotificationEndId { get; set; } = -1;

    public static Course CreateNewInstance(Guid termId, Guid instructorId, string title, CourseStatus status, DateTime startDate, DateTime endDate, string? notes = null)
    {
        return new Course()
        {
            CourseId = Guid.NewGuid(),
            TermId = termId,
            InstructorId = instructorId,
            Title = title,
            Status = status,
            StartDate = startDate,
            EndDate = endDate,
            Notes = notes
        };
    }
    
    public static Course CreateInstance(Guid courseId, Guid termId, Guid instructorId, string title, CourseStatus status, DateTime startDate, DateTime endDate, string? notes = null)
    {
        return new Course()
        {
            CourseId = courseId,
            TermId = termId,
            InstructorId = instructorId,
            Title = title,
            Status = status,
            StartDate = startDate,
            EndDate = endDate,
            Notes = notes
        };
    }
}