using System.ComponentModel.DataAnnotations.Schema;

namespace WGU_App_RileyJuniewic.Data.Models;

[Table("User")]
public class User
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    public static User CreateNewInstance(string username, string password)
    {
        return new User
        {
            UserId = Guid.NewGuid(),
            Username = username,
            Password = password
        };
    }

    public static User CreateInstance(Guid userId, string username, string password)
    {
        return new User
        {
            UserId = userId,
            Username = username,
            Password = password
        };
    }
}