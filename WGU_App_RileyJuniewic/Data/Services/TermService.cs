using Ardalis.Result;
using WGU_App_RileyJuniewic.Data.Dtos.Term;
using WGU_App_RileyJuniewic.Data.Models;
using WGU_App_RileyJuniewic.Data.Models.Interfaces;
using WGU_App_RileyJuniewic.Data.Repository;

namespace WGU_App_RileyJuniewic.Data.Services;

public interface ITermService
{
    Task<Result<List<Term>>> GetAllTermsAsync();
    Task<Result<Term>> GetTermAsync(Guid termId);
    Task<Result<Term>> CreateTermAsync(CreateTermRequest request);
    Task<Result<Term>> UpdateTermAsync(UpdateTermRequest request);
    Task<Result> DeleteTermAsync(Guid termId);
}

public class TermService(SqlDataAccessAsync sqlDataAccess, UserStore userStore) :
    ITermService,
    ICreateService<CreateTermRequest, Term>,
    IModifyService<UpdateTermRequest, Term>
{
    public async Task<Result<Term>> CreateTermAsync(CreateTermRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetErrorList());

        var user = userStore.GetUser();
        if (user.IsError())
            return Result.Error(new ErrorList(user.Errors));

        var term = Term.CreateNewInstance(request.Title, user.Value.UserId, request.StartDate, request.EndDate);
        var result = await ValidateTermAsync(term);
        if (result.IsError())
            return result;

        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.InsertAsync(term);
        return term;
    }

    public async Task<Result> DeleteTermAsync(Guid termId)
    {
        var user = userStore.GetUser();
        if (user.IsError())
            return Result.Error(new ErrorList(user.Errors));

        var connection = await sqlDataAccess.GetConnectionAsync();

        var term = await GetTermAsync(termId);
        if (term.IsError())
            return Result.Error(new ErrorList(term.Errors));

        // Delete Assesements & Notes
        var courses = await connection.Table<Course>().Where(x => x.TermId == termId).ToListAsync();
        foreach (var course in courses)
        {
        await connection.Table<Assessment>().Where(x => x.CourseId == course.CourseId).DeleteAsync();
        await connection.Table<Note>().Where(x => x.CourseId == course.CourseId).DeleteAsync();
        }
        // Delete Courses
        await connection.Table<Course>().Where(x => x.TermId == termId).DeleteAsync();

        // Delete Term
        await connection.Table<Term>().Where(x => x.TermId == termId && x.UserId == user.Value.UserId).DeleteAsync();

        return Result.Success();
    }

    public async Task<Result<Term>> GetTermAsync(Guid termId)
    {
        var user = userStore.GetUser();
        if (user.IsError())
            return Result.Error(new ErrorList(user.Errors));

        var connection = await sqlDataAccess.GetConnectionAsync();
        var term = await connection.Table<Term>().Where(x => x.TermId == termId && x.UserId == user.Value.UserId).FirstOrDefaultAsync();
        if (term == null)
            return Result.Error("Term not found");

        return term;
    }

    public async Task<Result<List<Term>>> GetAllTermsAsync()
    {
        var user = userStore.GetUser();
        if (user.IsError())
            return Result.Error(new ErrorList(user.Errors));
        var connection = await sqlDataAccess.GetConnectionAsync();
        return await connection.Table<Term>().Where(x => x.UserId == user.Value.UserId).ToListAsync();
    }

    public async Task<Result<Term>> UpdateTermAsync(UpdateTermRequest request)
    {
        if (request.HasErrors)
            return Result.Error(request.GetErrorList());
            
        var user = userStore.GetUser();
        if (user.IsError())
            return Result.Error(new ErrorList(user.Errors));

        var term = Term.CreateInstance(request.Id, user.Value.UserId, request.Title, request.StartDate, request.EndDate);
        var result = await ValidateTermAsync(term);
        if (result.IsError())
            return result;

        var connection = await sqlDataAccess.GetConnectionAsync();
        await connection.UpdateAsync(term);
        return term;
    }

    internal async Task<Result> ValidateTermAsync(Term term)
    {
        var user = userStore.GetUser();
        if (user.IsError())
            return Result.Error(new ErrorList(user.Errors));

        var connection = await sqlDataAccess.GetConnectionAsync();
        var existingTerms = await connection.Table<Term>().Where(x => x.UserId == user.Value.UserId).ToListAsync();
        foreach (var existingTerm in existingTerms)
        {
            if (existingTerm.StartDate <= term.EndDate && existingTerm.EndDate >= term.StartDate && existingTerm.TermId != term.TermId)
                return Result.Error("Term overlaps with an existing term");

            if (existingTerm.Title == term.Title && existingTerm.TermId != term.TermId)
                return Result.Error("Term with the same title already exists");
        }

        var monthSpan = term.EndDate.Month - term.StartDate.Month;
        if (monthSpan > 6)
            return Result.Error("Term must be less than 6 months");

            return Result.Success();
    }

    public async Task<Result<Term>> CreateAsync(CreateTermRequest request) => await CreateTermAsync(request);

    public async Task<Result<Term>> UpdateAsync(UpdateTermRequest request) => await UpdateTermAsync(request);

    public async Task<Result> DeleteAsync(Guid id) => await DeleteTermAsync(id);
}