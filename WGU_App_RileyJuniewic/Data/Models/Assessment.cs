using SQLite;
using WGU_App_RileyJuniewic.Data.Models.Enums;

namespace WGU_App_RileyJuniewic.Data.Models;

[Table("Assessment")]
public class Assessment
{
    [PrimaryKey]
    public Guid AssessmentId { get; set; }

    [Indexed]
    public Guid CourseId { get; set; }

    public string Name { get; set; } = "";
    public AssessmentType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public static Assessment CreateNewInstance(Guid courseId, string name, AssessmentType type, DateTime startDate, DateTime endDate)
    {
        return new Assessment
        {
            AssessmentId = Guid.NewGuid(),
            CourseId = courseId,
            Name = name,
            Type = type,
            StartDate = startDate,
            EndDate = endDate
        };
    }

    public static Assessment CreateInstance(Guid assessmentId, Guid courseId, string name, AssessmentType type, DateTime startDate, DateTime endDate)
    {
        return new Assessment
        {
            AssessmentId = assessmentId,
            CourseId = courseId,
            Name = name,
            Type = type,
            StartDate = startDate,
            EndDate = endDate
        };
    }
}