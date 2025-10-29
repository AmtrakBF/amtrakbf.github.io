using SQLite;

namespace WGU_App_RileyJuniewic.Data.Models;

[Table("Note")]
public class Note
{
    [PrimaryKey]
    public Guid NoteId { get; set; }
    
    [Indexed]
    public Guid CourseId { get; set; }
    
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";

    public static Note CreateNewInstance(string title, string content)
    {
        return new Note
        {
            NoteId = Guid.NewGuid(),
            Title = title,
            Content = content
        };
    }

    public static Note CreateInstance(Guid noteId, string title, string content)
    {
        return new Note
        {
            NoteId = noteId,
            Title = title,
            Content = content
        };
    }
}