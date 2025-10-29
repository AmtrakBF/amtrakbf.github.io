using SQLite;

namespace WGU_App_RileyJuniewic.Data.Models;

[Table("Term")]
public class Term
{
    [PrimaryKey]
    public Guid TermId { get; set; }
    public string Title { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public static Term CreateNewInstance(string title, DateTime startDate, DateTime endDate)
    {
        return new Term
        {
            TermId = Guid.NewGuid(),
            Title = title,
            StartDate = startDate,
            EndDate = endDate
        };
    }

    public static Term CreateInstance(Guid termId, string title, DateTime startDate, DateTime endDate)
    {
        return new Term
        {
            TermId = termId,
            Title = title,
            StartDate = startDate,
            EndDate = endDate
        };
    }
}