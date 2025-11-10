
namespace WGU_App_RileyJuniewic.Data.Dtos;

public class SearchResult
{
    public IEnumerable<Models.Term> Terms { get; set; } = new List<Models.Term>();
    public IEnumerable<Models.Course> Courses { get; set; } = new List<Models.Course>();
    public IEnumerable<Models.Assessment> Assessments { get; set; } = new List<Models.Assessment>();
    public IEnumerable<Models.Instructor> Instructors { get; set; } = new List<Models.Instructor>();
}