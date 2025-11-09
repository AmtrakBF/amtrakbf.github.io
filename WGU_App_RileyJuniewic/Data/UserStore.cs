using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Models;

namespace WGU_App_RileyJuniewic.Data;

public class UserStore
{
    private User _user = new();

    public Result<User> GetUser()
    {
        if (_user.UserId == Guid.Empty)
            return Result.Error("User is not logged in.");

        return _user;
    }

    public void SetUser(User user)
    {
        _user = user;
    }
}