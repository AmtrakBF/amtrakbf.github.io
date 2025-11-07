using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos.Instructor;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface IInstructorService
{
    Task<IEnumerable<Instructor>> GetAllInstructorsAsync();
    Task<Result<Instructor>> GetInstructorAsync(Guid id);
    Task<Result<Instructor>> CreateInstructorAsync(CreateInstructorRequest request);
    Task<Result<Instructor>> UpdateInstructorAsync(UpdateInstructorRequest request);
    Task<Result> DeleteInstructorAsync(Guid id);
}

public class InstructorService(SqlDataAccessAsync sqlDataAccess) :
    IInstructorService,
    ICreateService<CreateInstructorRequest, Instructor>,
    IModifyService<UpdateInstructorRequest, Instructor>
{
    public async Task<Result<Instructor>> CreateAsync(CreateInstructorRequest request) => await CreateInstructorAsync(request);

    public async Task<Result<Instructor>> CreateInstructorAsync(CreateInstructorRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetErrorList());

        var instructor = Instructor.CreateNewInstance(request.Name, request.Email, request.Phone);
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

        var coursesWithInstructor = await connection.Table<Course>().Where(x => x.InstructorId == id).ToListAsync();
        if (coursesWithInstructor.Any())
            return Result.Error("Cannot delete Instructor as they are assigned to a course");

        await connection.Table<Instructor>().Where(x => x.InstructorId == id).DeleteAsync();
        return Result.Success();
    }

    public async Task<IEnumerable<Instructor>> GetAllInstructorsAsync()
    {
        var connection = await sqlDataAccess.GetConnectionAsync();
        return await connection.Table<Instructor>().ToListAsync();
    }

    public async Task<Result<Instructor>> GetInstructorAsync(Guid id)
    {
        var connection = await sqlDataAccess.GetConnectionAsync();
        var instructor = await connection.Table<Instructor>().Where(x => x.InstructorId == id).FirstOrDefaultAsync();
        if (instructor == null)
            return Result.Error("Instructor not found");

        return instructor;
    }

    public async Task<Result<Instructor>> UpdateAsync(UpdateInstructorRequest request) => await UpdateInstructorAsync(request);

    public async Task<Result<Instructor>> UpdateInstructorAsync(UpdateInstructorRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetErrorList());
            
        var instructor = Instructor.CreateInstance(request.Id, request.Name, request.Email, request.Phone);
        var result = await ValidateInstructorAsync(instructor);
        if (result.IsError())
            return result;

        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.UpdateAsync(instructor);
        return instructor;
    }

    internal async Task<Result> ValidateInstructorAsync(Instructor instructor)
    {
        var connection = await sqlDataAccess.GetConnectionAsync();
        var existingEmail = await connection.Table<Instructor>().Where(x => x.Email == instructor.Email).FirstOrDefaultAsync();
        if (existingEmail != null && existingEmail.InstructorId != instructor.InstructorId)
            return Result.Error("Instructor with email already exists");

        var existingPhone = await connection.Table<Instructor>().Where(x => x.Phone == instructor.Phone).FirstOrDefaultAsync();
        if (existingPhone != null && existingPhone.InstructorId != instructor.InstructorId)
            return Result.Error("Instructor with phone number already exists");

        return Result.Success();
    }
}