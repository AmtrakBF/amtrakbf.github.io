using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos.Instructor;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface IInstructorService
{
    Task<Result<IEnumerable<Instructor>>> GetAllInstructorsAsync();
    Task<Result<Instructor>> GetInstructorAsync(Guid id);
    Task<Result<Instructor>> CreateInstructorAsync(CreateInstructorRequest request);
    Task<Result<Instructor>> UpdateInstructorAsync(UpdateInstructorRequest request);
    Task<Result> DeleteInstructorAsync(Guid id);
}

public class InstructorService(SqlDataAccessAsync sqlDataAccess, UserStore userStore) :
    IInstructorService,
    ICreateService<CreateInstructorRequest, Instructor>,
    IModifyService<UpdateInstructorRequest, Instructor>
{
    public async Task<Result<Instructor>> CreateAsync(CreateInstructorRequest request) => await CreateInstructorAsync(request);

    public async Task<Result<Instructor>> CreateInstructorAsync(CreateInstructorRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetErrorList());

        var user = userStore.GetUser();
        if (user.IsError())
            return Result.Error(new ErrorList(user.Errors));

        var instructor = Instructor.CreateNewInstance(user.Value.UserId, request.Name, request.Email, request.Phone);
        var result = await ValidateInstructorAsync(instructor);
        if (result.IsError())
            return result;

        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.InsertAsync(instructor);
        return instructor;
    }

    public async Task<Result> DeleteAsync(Guid id) => await DeleteInstructorAsync(id);

    public async Task<Result> DeleteInstructorAsync(Guid id)
    {
        var connection = await sqlDataAccess.GetConnectionAsync();

        var user = userStore.GetUser();
        if (user.IsError())
            return Result.Error(new ErrorList(user.Errors));

        var instructor = await GetInstructorAsync(id);
        if (instructor.IsError())
            return Result.Error(new ErrorList(instructor.Errors));

        var coursesWithInstructor = await connection.Table<Course>().Where(x => x.InstructorId == id).ToListAsync();
        if (coursesWithInstructor.Any())
            return Result.Error("Cannot delete Instructor as they are assigned to a course");

        await connection.Table<Instructor>().Where(x => x.InstructorId == id && x.UserId == user.Value.UserId).DeleteAsync();
        return Result.Success();
    }

    public async Task<Result<IEnumerable<Instructor>>> GetAllInstructorsAsync()
    {
        var user = userStore.GetUser();
        if (user.IsError())
            return Result.Error(new ErrorList(user.Errors));

        var connection = await sqlDataAccess.GetConnectionAsync();
        return await connection.Table<Instructor>().Where(x => x.UserId == user.Value.UserId).ToListAsync();
    }

    public async Task<Result<Instructor>> GetInstructorAsync(Guid id)
    {
        var user = userStore.GetUser();
        if (user.IsError())
            return Result.Error(new ErrorList(user.Errors));
            
        var connection = await sqlDataAccess.GetConnectionAsync();
        var instructor = await connection.Table<Instructor>().Where(x => x.InstructorId == id && x.UserId == user.Value.UserId).FirstOrDefaultAsync();
        if (instructor == null)
            return Result.Error("Instructor not found");

        return instructor;
    }

    public async Task<Result<Instructor>> UpdateAsync(UpdateInstructorRequest request) => await UpdateInstructorAsync(request);

    public async Task<Result<Instructor>> UpdateInstructorAsync(UpdateInstructorRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetErrorList());

        var user = userStore.GetUser();
        if (user.IsError())
            return Result.Error(new ErrorList(user.Errors));
            
        var instructor = Instructor.CreateInstance(request.Id, user.Value.UserId, request.Name, request.Email, request.Phone);
        var result = await ValidateInstructorAsync(instructor);
        if (result.IsError())
            return result;

        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.UpdateAsync(instructor);
        return instructor;
    }

    internal async Task<Result> ValidateInstructorAsync(Instructor instructor)
    {
        var user = userStore.GetUser();
        if (user.IsError())
            return Result.Error(new ErrorList(user.Errors));

        var connection = await sqlDataAccess.GetConnectionAsync();
        var existingEmail = await connection.Table<Instructor>().Where(x => x.Email == instructor.Email && x.UserId == user.Value.UserId).FirstOrDefaultAsync();
        if (existingEmail != null && existingEmail.InstructorId != instructor.InstructorId)
            return Result.Error("Instructor with email already exists");

        var existingPhone = await connection.Table<Instructor>().Where(x => x.Phone == instructor.Phone && x.UserId == user.Value.UserId).FirstOrDefaultAsync();
        if (existingPhone != null && existingPhone.InstructorId != instructor.InstructorId)
            return Result.Error("Instructor with phone number already exists");

        return Result.Success();
    }
}