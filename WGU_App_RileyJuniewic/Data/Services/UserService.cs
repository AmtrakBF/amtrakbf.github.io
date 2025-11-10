using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface IUserService
{
    Task<Result<User>> CreateAsync(UserCreateRequest request);
    Task<Result<User>> LoginAsync(UserLoginRequest request);
    Task<Result<User>> GetAsync(Guid userId);
}

public class UserService(SqlDataAccessAsync sqlDataAccess) : ICreateService<UserCreateRequest, User>, IUserService
{
    public async Task<Result<User>> CreateAsync(UserCreateRequest request)
    {
        var connection = await sqlDataAccess.GetConnectionAsync();
        var existingUser = await connection.Table<User>().Where(x => x.Username == request.Username.ToLower()).FirstOrDefaultAsync();
        if (existingUser != null)
            return Result.Error("User already exists.");

        if (request.Password != request.ConfirmPassword)
            return Result.Error("Passwords do not match.");

        var user = User.CreateNewInstance(request.Username, request.Password);
        await connection.InsertAsync(user);

        return user;
    }

    public async Task<Result<User>> GetAsync(Guid userId)
    {
        var connection = await sqlDataAccess.GetConnectionAsync();
        var user = await connection.Table<User>().Where(x => x.UserId == userId).FirstOrDefaultAsync();
        if (user == null)
            return Result.Error("User not found.");

        return user;
    }

    public async Task<Result<User>> LoginAsync(UserLoginRequest request)
    {
        var connection = await sqlDataAccess.GetConnectionAsync();
        var user = await connection.Table<User>().Where(x => x.Username == request.Username.ToLower()).FirstOrDefaultAsync();
        if (user != null && user.MatchPassword(request.Password))
            return user;

        return Result.Error("Invalid username or password.");
    }
}