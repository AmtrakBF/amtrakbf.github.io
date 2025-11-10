using SQLite;

namespace WGU_App_RileyJuniewic.Data.Models;

[Table("Term")]
public class Term
{
    [PrimaryKey]
    public Guid TermId { get; set; }
    [Indexed]
    public Guid UserId { get; set; }
    public string Title { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public static Term CreateNewInstance(string title, Guid userId, DateTime startDate, DateTime endDate)
    {
        return new Term
        {
            TermId = Guid.NewGuid(),
            Title = title,
            StartDate = startDate,
            EndDate = endDate,
            UserId = userId
        };
    }

    public static Term CreateInstance(Guid termId, Guid userId, string title, DateTime startDate, DateTime endDate)
    {
        return new Term
        {
            TermId = termId,
            Title = title,
            StartDate = startDate,
            EndDate = endDate,
            UserId = userId
        };
    }
}