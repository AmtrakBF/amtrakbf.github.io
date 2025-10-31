using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos.Instructor;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface IInstructorService
{
    Task<IEnumerable<Instructor>> GetAllInstructorsAsync();
    Task<Result<Instructor>> GetInstructorAsync(Guid id);
    Task<Result<Instructor>> CreateInstructorAsync(CreateInstructorRequest request);
    Task<Result<Instructor>> UpdateInstructorAsync(UpdateInstructorRequest request);
    Task DeleteInstructorAsync(Guid id);
}

public class InstructorService(SqlDataAccessAsync sqlDataAccess) : IInstructorService
{
    public async Task<Result<Instructor>> CreateInstructorAsync(CreateInstructorRequest request)
    {
        var instructor = Instructor.CreateNewInstance(request.Name, request.Email, request.Phone);
        var result = await ValidateInstructorAsync(instructor);
        if (result.IsError())
            return result;

        await sqlDataAccess.GetConnection().InsertAsync(instructor);
        return instructor;
    }

    public async Task DeleteInstructorAsync(Guid id) =>
        await sqlDataAccess.GetConnection().Table<Instructor>().Where(x => x.InstructorId == id).DeleteAsync();

    public async Task<IEnumerable<Instructor>> GetAllInstructorsAsync() => await sqlDataAccess.GetConnection().Table<Instructor>().ToListAsync();

    public async Task<Result<Instructor>> GetInstructorAsync(Guid id)
    {
        var instructor = await sqlDataAccess.GetConnection().Table<Instructor>().Where(x => x.InstructorId == id).FirstOrDefaultAsync();
        if (instructor == null)
            return Result.Error("Instructor not found");

        return instructor;
    }

    public async Task<Result<Instructor>> UpdateInstructorAsync(UpdateInstructorRequest request)
    {
        var instructor = Instructor.CreateInstance(request.InstructorId, request.Name, request.Email, request.Phone);
        var result = await ValidateInstructorAsync(instructor);
        if (result.IsError())
            return result;

        await sqlDataAccess.GetConnection().UpdateAsync(instructor);
        return instructor;
    }

    internal async Task<Result> ValidateInstructorAsync(Instructor instructor)
    {
        var existingEmail = await sqlDataAccess.GetConnection().Table<Instructor>().Where(x => x.Email == instructor.Email).FirstOrDefaultAsync();
        if (existingEmail != null && existingEmail.InstructorId != instructor.InstructorId)
            return Result.Error("Instructor with email already exists");

        var existingPhone = await sqlDataAccess.GetConnection().Table<Instructor>().Where(x => x.Phone == instructor.Phone).FirstOrDefaultAsync();
        if (existingPhone != null && existingPhone.InstructorId != instructor.InstructorId)
            return Result.Error("Instructor with phone number already exists");

        return Result.Success();
    }
}