using SQLite;

namespace WGU_App_RileyJuniewic.Data.Models;

[Table("Instructor")]
public class Instructor
{
    [PrimaryKey]
    public Guid InstructorId { get; set; }
    [Indexed]
    public Guid UserId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";

    public static Instructor CreateNewInstance(Guid userId, string name, string email, string phone)
    {
        return new Instructor
        {
            InstructorId = Guid.NewGuid(),
            UserId = userId,
            Name = name,
            Email = email.ToLower(),
            Phone = phone
        };
    }

    public static Instructor CreateInstance(Guid instructorId, Guid userId,  string name, string email, string phone)
    {
        return new Instructor
        {
            InstructorId = instructorId,
            UserId = userId,
            Name = name,
            Email = email.ToLower(),
            Phone = phone
        };
    } 
}