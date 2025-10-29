using SQLite;

namespace WGU_App_RileyJuniewic.Data.Models;

[Table("Instructor")]
public class Instructor
{
    [PrimaryKey]
    public Guid InstructorId { get; set; }

    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";

    public static Instructor CreateNewInstance(string name, string email, string phone)
    {
        return new Instructor
        {
            InstructorId = Guid.NewGuid(),
            Name = name,
            Email = email,
            Phone = phone
        };
    }

    public static Instructor CreateInstance(Guid instructorId, string name, string email, string phone)
    {
        return new Instructor
        {
            InstructorId = instructorId,
            Name = name,
            Email = email,
            Phone = phone
        };
    } 
}